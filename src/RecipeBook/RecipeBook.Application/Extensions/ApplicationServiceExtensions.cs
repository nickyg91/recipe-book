using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.Services.Authentication;
using RecipeBook.Application.Services.Email;
using RecipeBook.Application.Services.Recipes;
using RecipeBook.Application.Services.User;
using RecipeBook.Domain.Authentication;
using RecipeBook.Domain.Email;
using RecipeBook.Domain.Recipe;
using RecipeBook.Domain.User;

namespace RecipeBook.Application.Extensions;

public static class ApplicationServiceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationServices()
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRecipeBookService, RecipeBookService>();
            return services;
        }
    }
}