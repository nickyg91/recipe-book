using System.Net.Mail;
using RecipeBook.Domain.Email;

namespace RecipeBook.Application.Services.Email;

public class EmailService(SmtpSettings settings) : IEmailService
{
    public async Task SendEmail(MailMessage mailMessage)
    {
        using SmtpClient smtpClient = new SmtpClient(settings.Host, settings.Port);
        await smtpClient.SendMailAsync(mailMessage);
    }
}