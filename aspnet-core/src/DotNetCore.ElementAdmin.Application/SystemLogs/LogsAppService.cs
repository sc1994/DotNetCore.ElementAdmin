using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using DotNetCore.ElementAdmin.Core.SystemLogs;
using DotNetCore.ElementAdmin.SystemLogs.Elasticsearch;
using DotNetCore.ElementAdmin.SystemLogs.Dto;

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

        public async Task<List<string>> GetAllIndexTimes()
        {
            var times = await _logManager.GetAllIndexTimes();
            var orderTimes = times.OrderBy(x => x);
            return new List<string>
            {
                orderTimes.FirstOrDefault().ToString("yyyy-MM-dd HH:mm"),
                orderTimes.LastOrDefault().ToString("yyyy-MM-dd HH:mm")
             };
        }

        public async Task<Dictionary<string, object>> PostAggregation(LogFiltrateInputDto input)
        {
            var search = GetElasticsearchByInput(input);

            search.Query.Bool.Must.Add(new ElasticsearchMust // 只查看 api 屏蔽 类型以swagger的静态资源访问日志
            {
                Prefix = new ElasticsearchPrefix
                {
                    FieldsRequestPathKeyword = $"/api/"
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

            // search.Aggs.Add("aggMessageTemplate", new ElasticsearchAggs
            // {
            //     Terms = new ElasticsearchTerms
            //     {
            //         Field = "messageTemplate.keyword",
            //         Size = 1500
            //     }
            // }); TODO:模板统计

            var agg = await _logManager.GetAggregation(search);
            var requestPath = Group(agg.requestPath, '/', 0);
            var context = Group(agg.context, '.', 0);
            return new Dictionary<string, object>
            {
                { "requestPath", requestPath },
                { "context", context },
                // { "messageTemplate", agg.messageTemplate } TODO:模板统计
            };
        }

        public async Task<string> PostSearch(LogFiltrateInputDto input)
        {
            var search = GetElasticsearchByInput(input);
            if (input.RequestPath?.Length > 0)
            {
                search.Query.Bool.Must.Add(new ElasticsearchMust
                {
                    Prefix = new ElasticsearchPrefix
                    {
                        FieldsRequestPathKeyword = $"/api/{string.Join("/", input.RequestPath.Where(x => !string.IsNullOrWhiteSpace(x)))}"
                    }
                });
            }

            if (input.Context?.Length > 0)
            {
                search.Query.Bool.Must.Add(new ElasticsearchMust
                {
                    Prefix = new ElasticsearchPrefix
                    {
                        FieldsSourceContextKeyword = $"{string.Join(".", input.Context.Where(x => !string.IsNullOrWhiteSpace(x)))}"
                    }
                });
            }
            search.Sort = new[]
            {
                new ElasticsearchSort
                {
                    Timestamp = new ElasticsearchTimestamp
                    {
                        Order = "DESC"
                    }
                }
            };
            return await _logManager.GetSearch(search);
        }

        private Elasticsearch GetElasticsearchByInput(LogFiltrateInputDto input)
        {
            var size = (input.PageIndex - 1) * input.PageSize;
            if (size < 0) size = 0;
            var search = new Elasticsearch
            {
                Query = new ElasticsearchQuery
                {
                    Bool = new ElasticsearchBool
                    {
                        Must = new List<ElasticsearchMust>()
                    }
                },
                Size = input.PageSize,
                From = size
            };

            DateTime? start = null, end = null;
            if (input.Times?.Length > 1)
            {
                start = input.Times[0].DateTime;
                end = input.Times[1].DateTime;
            }
            else if (input.TimeSelect > 0)
            {
                start = DateTime.Now.AddMinutes(-input.TimeSelect);
                end = DateTime.Now;
            }
            if (start != null && end != null)
            {
                search.Query.Bool.Must.Add(new ElasticsearchMust
                {
                    Range = new ElasticsearchRange
                    {
                        Timestamp = new ElasticsearchTimestamp
                        {
                            Gt = start.Value,
                            Lt = end.Value
                        }
                    }
                });
            }
            else
            {
                throw new Exception("plase select time range");
            }
            if (!string.IsNullOrWhiteSpace(input.Lv))
            {
                search.Query.Bool.Must.Add(new ElasticsearchMust
                {
                    Term = new ElasticsearchTerm
                    {
                        LevelKeyword = input.Lv
                    }
                });
            }

            if (!string.IsNullOrWhiteSpace(input.RequestId))
            {
                search.Query.Bool.Must.Add(new ElasticsearchMust
                {
                    Term = new ElasticsearchTerm
                    {
                        FieldsRequestIdKeyword = input.RequestId
                    }
                });
            }

            return search;
        }

        private IEnumerable<dynamic> Group(ElasticsearchBucket[] bucket, char split, int index)
        {
            var result = bucket.GroupBy(x => x.Key.Trim(split).Split(split)[index])
                               .Select(x =>
                               {
                                   var g = bucket.Where(
                                       w => w.Key.Trim(split).Split(split).Length > index + 1
                                       && w.Key.Trim(split).Split(split)[index] == x.Key
                                   ).ToArray();
                                   dynamic[] children = null;
                                   if (g.Length > 0)
                                   {
                                       children = Group(g, split, index + 1).ToArray();
                                   }
                                   var t = new
                                   {
                                       Label = x.Key,
                                       DocCount = x.Sum(s => s.DocCount),
                                       Value = x.Key,
                                       Children = children
                                   };
                                   return t;
                               }).ToList();
            return result;
        }
    }
}