using HB.Case.Api.Extensions;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace HB.Case.Api.DbRepositories
{
    public interface ICacheRepository
    {
        Task<T> Get<T>(string key) where T : class;
        Task<bool> Remove(string key);
        Task Set<T>(string key, T value) where T : class;
    }

    public class RedisRepository : ICacheRepository
    {
        private readonly ILogger<RedisRepository> _logger;
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly TimeSpan _expiry;


        public RedisRepository(ILogger<RedisRepository> logger, IConnectionMultiplexer multiplexer)
        {
            _logger = logger;
            _multiplexer = multiplexer;
            _expiry = TimeSpan.FromMinutes(5);
        }

        public async Task<T> Get<T>(string key) where T : class
        {
            T result = default;
            try
            {
                IDatabase db = _multiplexer.GetDatabase();

                if (db.IsConnected(key))
                {
                    RedisValue redisValue = await db.StringGetAsync(key);
                    result = redisValue.HasValue ? redisValue.Deserialize<T>() : null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error");
            }
            return result;
        }

        public async Task Set<T>(string key, T value) where T : class
        {
            try
            {
                IDatabase db = _multiplexer.GetDatabase();
                if (db.IsConnected(key))
                {
                    var jsonValue = JsonSerializer.Serialize(value);
                    await db.StringSetAsync(key, jsonValue, _expiry);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "error");
            }

        }

        public async Task<bool> Remove(string key)
        {
            bool result = false;

            try
            {
                IDatabase db = _multiplexer.GetDatabase();
                if (db.IsConnected(key))
                    result = await db.KeyDeleteAsync(key);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "error");
            }

            return result;
        }
    }
}
