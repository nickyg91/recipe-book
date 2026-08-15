using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.Services.Authentication;

namespace RecipeBook.Application.Extensions;

public static class AuthenticationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddTokenService()
        {
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}