using System.Collections.Generic;
using DotNetCore.ElementAdmin.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DotNetCore.ElementAdmin.Core.Caches
{
    public static class RedisCache
    {
        private static readonly object _createMultiplexerLock = new object();
        private static ConnectionMultiplexer _connectionMultiplexer;

        private static ConnectionMultiplexer ConnectionMultiplexer
        {
            get
            {
                if (_connectionMultiplexer != null) return _connectionMultiplexer;
                lock (_createMultiplexerLock)
                {
                    if (_connectionMultiplexer != null) return _connectionMultiplexer;
                    var config = AppConfigurations.Get("ConnectionStrings").GetSection("Redis").Value;
                    var options = ConfigurationOptions.Parse(config);

                    options.KeepAlive = 15;
                    options.ResolveDns = false;
                    options.AbortOnConnectFail = false;

                    _connectionMultiplexer = ConnectionMultiplexer.Connect(options);

                    _connectionMultiplexer.ErrorMessage += (errorMessage, e) =>
                    {
                        //TODO:记录异常
                        _connectionMultiplexer = null; // 释放
                    };
                    _connectionMultiplexer.InternalError += (errorMessage, e) =>
                    {
                        //TODO:记录异常
                        _connectionMultiplexer = null; // 释放
                    };

                    return _connectionMultiplexer;
                }
            }
        }

        public static IEnumerable<T> SetMembers<T>(string key)
        {
            var db = ConnectionMultiplexer.GetDatabase();
            var list = db.SetMembers(key);
            foreach (var item in list)
            {
                yield return JsonConvert.DeserializeObject<T>(item);
            }
        }
    }
}