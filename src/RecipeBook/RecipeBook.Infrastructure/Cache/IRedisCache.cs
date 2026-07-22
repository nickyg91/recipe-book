namespace RecipeBook.Infrastructure.Cache;

public interface IRedisCache<T>
{
    Task<T?> GetAsync(string key, CancellationToken cancellationToken = default);
    Task SetAsync(string key, T value, int expirationMinutes, CancellationToken cancellationToken = default);
}
