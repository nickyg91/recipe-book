using System.Net.Mail;

namespace RecipeBook.Domain.Email;

public class SmtpClientFactory 
{
    private readonly SmtpSettings _settings;

    public SmtpClientFactory(SmtpSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public SmtpClient CreateSmtpClient()
    {
        return new SmtpClient(_settings.Host, _settings.Port);
    }
}
