using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace RecipeBook.Infrastructure.Cache;

internal sealed class RedisCacheService : IRedisCache<Guid>
{
    private readonly ConnectionMultiplexer? _connectionMultiplexer;
    private readonly object? _lock = new();
    private IDatabase _redisDatabase;
    private Lazy<Task<IDatabase>> _databaseLazy = new(() => Connect());

    public RedisCacheService(ConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    private Task<IDatabase> Connect()
    {
        if (_connectionMultiplexer is null)
        {
            return Task.FromResult(_redisDatabase);
        }

        lock (_lock)
        {
            if (_redisDatabase is not null)
            {
                return Task.FromResult(_redisDatabase);
            }
            
            _redisDatabase = _connectionMultiplexer.GetDatabase();
            return Task.FromResult(_redisDatabase);
        }
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var db = await _databaseLazy.Value;
        string? val = await db.StringGetAsync(key);
        if (string.IsNullOrEmpty(val))
        {
            return default;
        }
        try
        {
            return JsonSerializer.Deserialize<T>(val)!;
        }
        catch
        {
            return (T?)val;
        }
    }

    public async Task SetAsync(string key, string value, int expirationMinutes, CancellationToken cancellationToken = default)
    {
        var db = await _databaseLazy.Value;
        var bytes = Encoding.UTF8.GetBytes(serializedValue);

        await db.KeySetAsync(key, bytes, TimeSpan.FromMinutes(expirationMinutes), cancellationToken: cancellationToken);
    }
}

file class RedisConnectionException : Exception
{
    public RedisConnectionException() : base("Failed to connect to Redis.") { }
}
