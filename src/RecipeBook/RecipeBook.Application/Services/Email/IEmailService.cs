namespace RecipeBook.Application.Services.Email;

public interface IEmailService
{
    Task SendEmail(string to, string from, string subject, string body);
}