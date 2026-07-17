using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.Domain.Services.User;
using RecipeBook.Application.Dto;
using RecipeBook.Application.Exceptions;
using RecipeBook.Application.Mappers;
using RecipeBook.Application.Security;
using RecipeBook.Application.Services.Email;
using RecipeBook.Domain.Entities.Users;
using RecipeBook.Infrastructure.Database.Context.RecipeBook;

namespace RecipeBook.Application.Services.User;

internal sealed class UserService(
    RecipeBookDbContext dbContext, 
    IEmailService emailService,
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
        dbContext.Users.Add(userEntity);
        await dbContext.SaveChangesAsync(cancellationToken);

        string body = $"Please confirm your account by clicking the following link: <a src='{frontendUrl}/confirm-account/{userEntity.EmailConfirmationToken}'>Confirm account.</a>";
        
        MailMessage message = new("no-reply@recipebook.nickganter.dev", userDto.Email, "Confirm Account", body);
        message.To.Add(userDto.Email);
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

        return !PasswordHasher.VerifyPassword(password, user.Password)
            ? throw new InvalidPasswordException("The provided password is incorrect.")
            : _mapper.ToUserDto(user);
    }

    private async Task<bool> IsEmailTakenAsync(string email, CancellationToken cancellationToken)
    {
        return await dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken);
    }
}