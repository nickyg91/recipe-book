using StackExchange.Redis;
using System.Text.Json;

namespace RecipeBook.Infrastructure.Cache;

internal sealed class RedisCacheService(Lazy<ConnectionMultiplexer> connectionMultiplexer) : IRedisCache
{
    private readonly Lazy<IDatabase> _databaseLazy = new(() => connectionMultiplexer.Value.GetDatabase());

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

    public async Task RemoveAsync(string key)
    {
        await _databaseLazy.Value.KeyDeleteAsync(key);
    }
}
