using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using StackExchange.Redis;

namespace RecipeBook.Infrastructure.Cache;

internal sealed class RedisCacheService : IRedisCache
{
    private readonly string _setKeyPrefix = "tags:";
    private readonly Lazy<IDatabase> _databaseLazy;

    public RedisCacheService(Lazy<ConnectionMultiplexer> connectionMultiplexer)
    {
        ConnectionMultiplexer redis = connectionMultiplexer.Value;
        _databaseLazy = new Lazy<IDatabase>(() => redis.GetDatabase());
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        IDatabase db = _databaseLazy.Value;
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
            return (T)(object)val!;
        }
    }

    public async Task SetAsync<T>(string key, T value, int? expirationMinutes)
    {
        IDatabase db = _databaseLazy.Value;
        string serializedValue = JsonSerializer.Serialize(value);
        if (expirationMinutes.HasValue)
        {
            await db.StringSetAsync(key, serializedValue, TimeSpan.FromMinutes(expirationMinutes.Value));
        }
        else
        {
            await db.StringSetAsync(key, serializedValue);
        }
    }

    public async Task AddToSetAsync<T>(string key, T value)
    {
        
        string serializedValue = JsonSerializer.Serialize(value);
        await _databaseLazy.Value.SetAddAsync(key, serializedValue);
    }

    public async Task RemoveAsync(string key)
    {
        await _databaseLazy.Value.KeyDeleteAsync(key);
    }

    /// <summary>
    /// Adds a tag to a recipe's tag set using Redis native SET (SADD).
    /// </summary>
    public async Task AddTagAsync(string recipeId, string tagName)
    {
        IDatabase db = _databaseLazy.Value;
        string key = $"{_setKeyPrefix}{recipeId}";
        await db.SetAddAsync(key, tagName);
    }

    /// <summary>
    /// Removes a tag from a recipe's tag set using Redis native SET (SREM).
    /// </summary>
    public async Task RemoveFromSetAsync(string recipeId, string tagName)
    {
        IDatabase db = _databaseLazy.Value;
        string key = $"{_setKeyPrefix}{recipeId}";

        await db.SetRemoveAsync(key, tagName);
    }

    /// <summary>
    /// Scans the full Redis SET for a recipe, yielding all tag names.
    /// Uses cursor-based SCAN to handle sets larger than 128K entries.
    /// </summary>
    public async IAsyncEnumerable<string> SetScanAsync(
        string recipeId, [EnumeratorCancellation] CancellationToken ct = default)
    {
        var key = $"{_setKeyPrefix}{recipeId}";

        await foreach (RedisValue item in _databaseLazy.Value.SetScanAsync(key).WithCancellation(ct))
        {
            yield return item!;
        }
    }
}
