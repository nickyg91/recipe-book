using System.Net.Mail;

namespace RecipeBook.Application.Services.Email;

public interface IEmailService
{
    Task SendEmail(MailMessage mailMessage);
}