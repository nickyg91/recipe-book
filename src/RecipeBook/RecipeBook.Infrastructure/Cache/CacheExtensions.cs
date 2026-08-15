using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace RecipeBook.Infrastructure.Cache;

public static class CacheExtensions
{
    public static IServiceCollection AddRedisCache(this IServiceCollection services, string connectionString)
    {
        ConfigurationOptions configuration = ConfigurationOptions.Parse(connectionString);
        Lazy<ConnectionMultiplexer>  lazyConnection = new Lazy<ConnectionMultiplexer>(() =>  ConnectionMultiplexer.Connect(configuration));
        services.AddSingleton(lazyConnection);
        services.AddSingleton<IRedisCache, RedisCacheService>();
        return services;
    }
}
