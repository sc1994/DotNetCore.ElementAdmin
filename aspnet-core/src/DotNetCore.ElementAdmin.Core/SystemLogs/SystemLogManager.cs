using System;
using Flurl.Http;
using System.Linq;
using Newtonsoft.Json;
using Abp.Domain.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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

        public async Task<(ElasticsearchBucket[] messageTemplate, ElasticsearchBucket[] context, ElasticsearchBucket[] requestPath)> GetAggregation(Elasticsearch input)
        {
            var count = 0;
        Again:
            var resultStr = await ToSearchAsync(input);

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
            var requestPaths = result.Aggregations["aggRequestPath"]
                                     .Buckets
                                     .Select(x =>
                                     {
                                         if (x.Key.StartsWith("/api/"))
                                             x.Key = x.Key.Replace("/api/", ""); // 移除统一前缀
                                         return x;
                                     })
                                     .ToArray();
            return (
                null, // result.Aggregations["aggMessageTemplate"].Buckets, 暂不查询
                result.Aggregations["aggSourceContext"].Buckets,
                requestPaths
            );
        }

        public async Task<string> GetSearch(Elasticsearch input)
        {
            return await ToSearchAsync(input);
        }

        private async Task<string> ToSearchAsync(Elasticsearch input)
        {
            var index = GetIndexs(input);
            var request = input;
            var url = $"{_elasticsearchPath}/{index}/logevent/_search";
            _log.LogInformation("Send {request} To {url}", new object[] { request, url });

            var response = await $"{_elasticsearchPath}/{index}/logevent/_search"
                                    .WithHeader("Content-Type", "application/json")
                                    .PostStringAsync(input.ToJson());
            return await response.Content.ReadAsStringAsync();
        }

        private string GetIndexs(Elasticsearch input)
        {
            var timeRange = input.Query.Bool.Must.FirstOrDefault(
                x => x.Range?.Timestamp?.Gt != null && x.Range?.Timestamp?.Lt != null
            ).Range.Timestamp;


            if (timeRange == null ||
                (timeRange.Gt - timeRange.Lt).Value.TotalDays > 10)
            {
                return $"logstash-.*";
            }

            var start = timeRange.Gt.Value.DateTime;
            var end = timeRange.Lt.Value.DateTime;

            var index = string.Empty;
            var tempDate = start.Date;
            do
            {
                index += $"logstash-{tempDate:yyyy.MM.dd},";
                tempDate = tempDate.AddDays(1);
            }
            while (tempDate <= end.Date && tempDate <= DateTime.Today);

            return index.TrimEnd(',');
        }
    }
}