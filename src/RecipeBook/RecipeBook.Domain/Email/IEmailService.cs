using System.Net.Mail;

namespace RecipeBook.Domain.Email;

public interface IEmailService
{
    Task SendEmail(MailMessage mailMessage);
}