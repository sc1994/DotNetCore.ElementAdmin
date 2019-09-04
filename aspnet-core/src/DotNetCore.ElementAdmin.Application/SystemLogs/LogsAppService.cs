using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using DotNetCore.ElementAdmin.Core.SystemLogs;
using DotNetCore.ElementAdmin.SystemLogs.Elasticsearch;

namespace DotNetCore.ElementAdmin.Application.SystemLogs
{
    public class LogsAppService : ApplicationService, ILogsAppService
    {
        private readonly ILogger<LogsAppService> _log;
        private readonly SystemLogManager _logManager;

        public LogsAppService(
            ILogger<LogsAppService> log,
            SystemLogManager logManager
        )
        {
            _log = log;
            _logManager = logManager;
        }

        public async Task<object> GetAggregation(DateTime startTime, DateTime endTime)
        {
            var search = new Elasticsearch
            {
                Query = new ElasticsearchQuery
                {
                    Bool = new ElasticsearchBool
                    {
                        Must = new List<ElasticsearchMust>()
                    }
                },
                Size = 0,
            };

            search.Query.Bool.Must.Add(new ElasticsearchMust
            {
                Range = new ElasticsearchRange
                {
                    Timestamp = new ElasticsearchTimestamp
                    {
                        Gt = startTime,
                        Lt = endTime
                    }
                }
            });

            // 添加内容聚合
            search.Aggs.Add("aggSourceContext", new ElasticsearchAggs
            {
                Terms = new ElasticsearchTerms
                {
                    Field = "fields.SourceContext.keyword",
                    Size = 1500
                }
            });

            search.Aggs.Add("aggRequestPath", new ElasticsearchAggs
            {
                Terms = new ElasticsearchTerms
                {
                    Field = "fields.RequestPath.keyword",
                    Size = 1500
                }
            });

            search.Aggs.Add("aggMessageTemplate", new ElasticsearchAggs
            {
                Terms = new ElasticsearchTerms
                {
                    Field = "messageTemplate.keyword",
                    Size = 1500
                }
            });

            var agg = await _logManager.GetAggregation(search);
            var debugger = Group(agg.requestPath, '/', 0);
            return debugger;
        }

        private IEnumerable<dynamic> Group(ElasticsearchBucket[] bucket, char split, int index)
        {
            var result = bucket.GroupBy(x => x.Key.Split(split)[index])
                               .Select(x =>
                               {
                                   var g = bucket.Where(
                                       w => w.Key.Split(split).Length > index + 1
                                       && w.Key.Split(split)[index] == x.Key
                                   ).ToArray();
                                   var label = x.Key;
                                   var children = new dynamic[0];
                                   if (g.Length > 0)
                                   {
                                       children = Group(g, split, index + 1).ToArray();
                                   }
                                   var t = new
                                   {
                                       Label = label,
                                       Value = x.Key,
                                       Children = children
                                   };
                                   return t;
                               });
            return result;
        }
    }
}