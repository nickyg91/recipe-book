using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.Dto;
using RecipeBook.Application.Exceptions;
using RecipeBook.Application.Mappers;
using RecipeBook.Application.Security;
using RecipeBook.Application.Services.Email;
using RecipeBook.Domain.Entities.Users;
using RecipeBook.Infrastructure.Cache;
using RecipeBook.Infrastructure.Database.Context.RecipeBook;

namespace RecipeBook.Application.Services.User;

internal sealed class UserService(
    RecipeBookDbContext dbContext,
    IEmailService emailService,
    IRedisCache redisCache,
    [FromKeyedServices("frontendUrl")] string frontendUrl) : IUserService
{
    private readonly UserDtoMapper _mapper = new();

    public async Task<bool> IsUsernameTakenAsync(string username, CancellationToken cancellationToken)
    {
        return await dbContext.Users.AnyAsync(x => x.Username == username, cancellationToken);
    }

    public async Task<UserDto> CreateUserAccountAsync(UserDto userDto, CancellationToken cancellationToken)
    {
        if (await IsEmailTakenAsync(userDto.Email, cancellationToken))
        {
            throw new UserExistsException("A user already exists with this email.");
        }
        UserEntity userEntity = _mapper.ToUserEntity(userDto);
        userEntity.Password = PasswordHasher.HashPassword(userDto.Password!);
        userEntity.EmailConfirmationToken = Guid.NewGuid();
        userEntity.Uuid = Guid.NewGuid();
        dbContext.Users.Add(userEntity);
        await dbContext.SaveChangesAsync(cancellationToken);

        string body = $"<!DOCTYPE html><html><body>Please confirm your account by clicking the following <a href=\"{frontendUrl}confirm-account/{userEntity.EmailConfirmationToken}\">link.</a></body></html>";
        
        MailMessage message = new("no-reply@recipebook.nickganter.dev", userDto.Email, "Confirm Account", body);
        message.To.Add(userDto.Email);
        message.IsBodyHtml = true;
        await emailService.SendEmail(message);
        return _mapper.ToUserDto(userEntity);
    }

    public async Task<UserDto> GetUserByEmailAndPasswordAsync(string email, string password,
        CancellationToken cancellationToken)
    {
        UserEntity? user =
            await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException("No user found with the provided email.");
        }

        if (!user.IsEmailConfirmed)
        {
            throw new AccountNotVerifiedException("The provided email is not confirmed.");
        }
        
        return !PasswordHasher.VerifyPassword(password, user.Password)
            ? throw new InvalidPasswordException("The provided password is incorrect.")
            : _mapper.ToUserDto(user);
    }

    public async Task<UserDto> GetUserByUuid(Guid uuid, CancellationToken cancellationToken)
    {
        UserEntity userEntity = await dbContext.Users.FirstOrDefaultAsync(x => x.Uuid == uuid, cancellationToken) ?? throw new UserNotFoundException($"User {uuid} not found.");  
        return _mapper.ToUserDto(userEntity);
    }

    public async Task ConfirmEmailAsync(Guid emailConfirmationToken, CancellationToken cancellationToken)
    {
        UserEntity? user = await dbContext.Users.FirstOrDefaultAsync(x => x.EmailConfirmationToken == emailConfirmationToken, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException("No user found with the provided email confirmation token.");
        }

        user.IsEmailConfirmed = true;
        user.EmailConfirmationToken = null;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<bool> IsEmailTakenAsync(string email, CancellationToken cancellationToken)
    {
        return await dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken);
    }

    public async Task RequestPasswordResetAsync(string email, CancellationToken cancellationToken)
    {
        UserEntity? user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (user == null)
        {
            return;
        }

        Guid token = Guid.NewGuid();
        string key = $"password-reset:{token}";
        await redisCache.SetAsync(key, user.Uuid.ToString(), 60);

        string body = $"""<!DOCTYPE html><html><body>Please reset your password by clicking the following <a href="{frontendUrl}reset-password/{token}">link.</a></body></html>""";

        MailMessage message = new("no-reply@recipebook.nickganter.dev", email, "Reset Password", body);
        message.To.Add(email);
        message.IsBodyHtml = true;
        await emailService.SendEmail(message);
    }

    public async Task ResetPasswordAsync(Guid token, string newPassword, CancellationToken cancellationToken)
    {
        string? uuidString = await redisCache.GetAsync<string>($"password-reset:{token}");

        if (uuidString == null)
        {
            throw new InvalidTokenException("The password reset token is invalid or has expired.");
        }

        Guid uuid = Guid.Parse(uuidString);
        UserEntity? user = await dbContext.Users.FirstOrDefaultAsync(x => x.Uuid == uuid, cancellationToken);

        if (user == null)
        {
            throw new InvalidTokenException("The password reset token is invalid or has expired.");
        }

        user.Password = PasswordHasher.HashPassword(newPassword);
        await redisCache.RemoveAsync($"password-reset:{token}");
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}