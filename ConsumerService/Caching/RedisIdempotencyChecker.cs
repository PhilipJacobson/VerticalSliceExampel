using StackExchange.Redis;

namespace ConsumerService.Caching;

public class RedisIdempotencyChecker
{
    private readonly IDatabase _db;

    public RedisIdempotencyChecker(ConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<bool> IsOperationAlreadyPerformedAsync(string messageId)
    {
        return await _db.KeyExistsAsync(messageId);
    }

    public async Task MarkOperationAsPerformedAsync(string messageId)
    {
        await _db.StringSetAsync(messageId, "", TimeSpan.FromHours(8));
    }
}
