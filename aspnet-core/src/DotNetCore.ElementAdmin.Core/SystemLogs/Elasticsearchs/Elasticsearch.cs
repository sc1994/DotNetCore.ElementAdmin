
using System.Collections.Generic;
namespace DotNetCore.ElementAdmin.SystemLogs.Elasticsearch
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Elasticsearch
    {
        [JsonProperty("query", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticsearchQuery Query { get; set; }

        [JsonProperty("aggs", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, ElasticsearchAggs> Aggs { get; set; } = new Dictionary<string, ElasticsearchAggs>();

        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public long? From { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("sort", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticsearchSort[] Sort { get; set; }

        [JsonProperty("highlight", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticsearchHighlight Highlight { get; set; }

        public static Elasticsearch FromJson(string json) => JsonConvert.DeserializeObject<Elasticsearch>(json, Converter.Settings);

        public string ToJson() => JsonConvert.SerializeObject(this, Converter.Settings);

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            };
        }
    }

    public partial class ElasticsearchHighlight
    {
        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticsearchHighlightFields Fields { get; set; }
    }

    public partial class ElasticsearchSort
    {
        [JsonProperty("@timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticsearchTimestamp Timestamp { get; set; }
    }

    public partial class ElasticsearchQuery
    {
        [JsonProperty("bool")]
        public ElasticsearchBool Bool { get; set; }
    }

    public partial class ElasticsearchAggs
    {
        [JsonProperty("terms")]
        public ElasticsearchTerms Terms { get; set; }
    }

    public partial class ElasticsearchTerms
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("collect_mode")]
        public string CollectMode { get; set; } = "breadth_first"; // 广度优先
    }

    public partial class ElasticsearchBool
    {
        [JsonProperty("must")]
        public List<ElasticsearchMust> Must { get; set; }
    }

    public partial class ElasticsearchMust
    {
        [JsonProperty("range", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticsearchRange Range { get; set; }

        [JsonProperty("term", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticsearchTerm Term { get; set; }

        [JsonProperty("match", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticsearchMatch Match { get; set; }

        [JsonProperty("prefix", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticsearchPrefix Prefix { get; set; }
    }

    public class ElasticsearchPrefix
    {
        [JsonProperty("fields.RequestPath.keyword", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldsRequestPathKeyword { get; set; }

        [JsonProperty("fields.SourceContext.keyword", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldsSourceContextKeyword { get; set; }
    }

    public class ElasticsearchMatch
    {
        [JsonProperty("fields.filter1.keyword", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldsFilter1Keyword { get; set; }
        [JsonProperty("fields.filter2.keyword", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldsFilter2Keyword { get; set; }
        [JsonProperty("fields.msg", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldsMsgKeyword { get; set; }
    }

    public partial class ElasticsearchRange
    {
        [JsonProperty("@timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticsearchTimestamp Timestamp { get; set; }
    }

    public partial class ElasticsearchTimestamp
    {
        [JsonProperty("gt", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Gt { get; set; }

        [JsonProperty("lt", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Lt { get; set; }

        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public string Order { get; set; }
    }

    public partial class ElasticsearchTerm
    {
        [JsonProperty("level.keyword", NullValueHandling = NullValueHandling.Ignore)]
        public string LevelKeyword { get; set; }
        [JsonProperty("fields.module.keyword", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldsModuleKeyword { get; set; }
        [JsonProperty("fields.category.keyword", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldsCategoryKeyword { get; set; }
        [JsonProperty("fields.sub_category.keyword", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldsSubCategoryKeyword { get; set; }
        [JsonProperty("fields.ip.keyword", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldsIpKeyword { get; set; }
        [JsonProperty("fields.app.keyword", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldsAppKeyword { get; set; }
    }

    public class ElasticsearchHighlightFields
    {
        [JsonProperty("fields.msg", NullValueHandling = NullValueHandling.Ignore)]
        public object FieldsMsg { get; set; } // todo 设置项
    }
}