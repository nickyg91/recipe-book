using RecipeBook.Application.Dto;

namespace RecipeBook.Application.Services.User;

public interface IUserService
{
    Task<bool> IsUsernameTakenAsync(string username, CancellationToken cancellationToken);
    Task<UserDto> CreateUserAccountAsync(UserDto userDto, CancellationToken cancellationToken);
    Task<UserDto> GetUserByEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken);
    Task ConfirmEmailAsync(Guid emailConfirmationToken, CancellationToken cancellationToken);
}