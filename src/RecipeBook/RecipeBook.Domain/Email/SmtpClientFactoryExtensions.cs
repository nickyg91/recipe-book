using Microsoft.Extensions.DependencyInjection;

namespace RecipeBook.Domain.Email;

public static class SmtpClientFactoryExtensions
{
    public static IServiceCollection AddSmtpClient(this IServiceCollection services, SmtpSettings settings)
    {
        services.AddSingleton(settings);
        services.AddSingleton<SmtpClientFactory>();
        return services;
    }
}
