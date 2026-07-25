namespace RecipeBook.Infrastructure.Cache;

public interface IRedisCache
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, int? expirationMinutes);
    Task RemoveAsync(string key);
}
