using StackExchange.Redis;
using System.Text.Json;

namespace HB.Case.Api.Extensions
{
    public static class RedisValueExtensions
    {
        public static T Deserialize<T>(this RedisValue redisValue)
        {
            T result = JsonSerializer.Deserialize<T>(redisValue);
            return result;
        }

    }
}
