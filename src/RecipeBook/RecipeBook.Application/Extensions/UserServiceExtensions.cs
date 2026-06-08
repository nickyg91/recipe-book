using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.Domain.Services.User;
using RecipeBook.Application.Services.User;

namespace RecipeBook.Application.Extensions;

public static class UserServiceExtensions
{
    public static IServiceCollection AddUserServices(this IServiceCollection serviceProvider)
    {
        serviceProvider.AddScoped<IUserService, UserService>();
        return serviceProvider;
    }
}