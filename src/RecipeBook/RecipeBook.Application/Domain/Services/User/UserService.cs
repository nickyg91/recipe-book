using Microsoft.EntityFrameworkCore;
using RecipeBook.Application.Domain.Dto;
using RecipeBook.Application.Exceptions;
using RecipeBook.Application.Mappers;
using RecipeBook.Application.Security;
using RecipeBook.Infrastructure.Database.Context.RecipeBook;

namespace RecipeBook.Application.Domain.Services.User;

internal class UserService(RecipeBookDbContext dbContext) : IUserService
{
    private readonly RecipeBookDbContext _dbContext = dbContext;
    private readonly UserDtoMapper _mapper = new();

    public async Task<bool> IsUsernameTakenAsync(string username, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.AnyAsync(x => x.Username == username, cancellationToken);
    }

    public async Task<UserDto> CreateUserAccountAsync(UserDto userDto, CancellationToken cancellationToken)
    {
        if (await IsEmailTakenAsync(userDto.Email, cancellationToken))
        {
            throw new UserExistsException("A user already exists with this email.");
        }
        RecipeBook.Domain.Entities.User userEntity = _mapper.ToUserEntity(userDto);
        userEntity.Password = PasswordHasher.HashPassword(userDto.Password!);
        _dbContext.Users.Add(userEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return _mapper.ToUserDto(userEntity);
    }

    public async Task<UserDto> GetUserByEmailAndPasswordAsync(string email, string password,
        CancellationToken cancellationToken)
    {
        RecipeBook.Domain.Entities.User? user =
            await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

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
        return await _dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken);
    }
}