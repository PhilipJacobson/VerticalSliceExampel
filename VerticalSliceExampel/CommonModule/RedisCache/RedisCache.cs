using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace VerticalSliceExampel.CommonModule.RedisCache;

public class RedisCache : IRedisCache
{
    private readonly IDatabase _database;

    public RedisCache()
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
        _database = redis.GetDatabase();
    }

    public async Task SetAsync<T>(string key, T value)
    {
        string serializedValue = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, serializedValue);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        string value = await _database.StringGetAsync(key);
        if (value == null)
        {
            return default(T);
        }

        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}
