using System;
using Flurl.Http;
using System.Linq;
using Newtonsoft.Json;
using Abp.Domain.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using DotNetCore.ElementAdmin.SystemLogs.Elasticsearch;

namespace DotNetCore.ElementAdmin.Core.SystemLogs
{
    public class SystemLogManager : DomainService
    {
        private string _elasticsearchPath = "http://localhost:9222";
        private readonly ILogger<SystemLogManager> _log;

        public SystemLogManager(
            ILogger<SystemLogManager> log
        )
        {
            _log = log;
        }

        public async Task<(ElasticsearchBucket[] messageTemplate, ElasticsearchBucket[] content, ElasticsearchBucket[] requestPath)> GetAggregation(Elasticsearch input)
        {
            var count = 0;
        Again:
            var timeRange = input.Query.Bool.Must.FirstOrDefault(
                x => x.Range?.Timestamp?.Gt != null && x.Range?.Timestamp?.Lt != null
            ).Range.Timestamp;

            var index = GetIndexs(timeRange.Gt.Value.DateTime, timeRange.Lt.Value.DateTime);

            var response = await $"{_elasticsearchPath}/{index}/logevent/_search"
                                    .WithHeader("Content-Type", "application/json")
                                    .PostStringAsync(input.ToJson());
            var resultStr = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ElasticsearchAggregations>(resultStr);
            if (result.Shards != null && result.Shards.Failed > 0)
            {
                foreach (var item in result.Shards.Failures)
                {
                    var res = await $"{_elasticsearchPath}/{item.Index}/_mapping/logevent"
                                    .WithHeader("Content-Type", "application/json")
                                    .PostJsonAsync(new
                                    {
                                        properties = input.Aggs.ToDictionary(
                                            x =>
                                            {
                                                var arr = x.Value.Terms.Field.Split('.');
                                                if (arr.LastOrDefault() == "keyword")
                                                {
                                                    arr = arr.Take(arr.Length - 1).ToArray();
                                                }
                                                return string.Join(".", arr);
                                            }, x => new
                                            {
                                                type = "text",
                                                fielddata = true
                                            }
                                        )
                                    });
                    _log.LogWarning("Set Es Mapping {Index} {Response}", new object[]
                    {
                        item.Index,
                        await res.Content.ReadAsStringAsync()
                    });
                    if (count < 3) // 重新刷数据
                    {
                        count++;
                        goto Again;
                    }
                }
            }
            return (
                result.Aggregations["aggMessageTemplate"].Buckets,
                result.Aggregations["aggSourceContext"].Buckets,
                result.Aggregations["aggRequestPath"].Buckets
            );
        }

        private string GetIndexs(DateTime start, DateTime end)
        {
            var index = string.Empty;
            var tempDate = start.Date;
            do
            {
                index += $"logstash-{tempDate:yyyy.MM.dd},";
                tempDate = tempDate.AddDays(1);
            }
            while (tempDate <= end.Date && tempDate <= DateTime.Today);
            if (index.Length > 300) // TODO: 防止太长
            {
                index = $"logstash-{end:yyyy.MM}.*";
            }
            return index.TrimEnd(',');
        }
    }
}