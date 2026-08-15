using RecipeBook.Domain.Dto;

namespace RecipeBook.Domain.User;

public interface IUserService
{
    Task<bool> IsUsernameTakenAsync(string username, CancellationToken cancellationToken);
    Task<UserDto> CreateUserAccountAsync(UserDto userDto, CancellationToken cancellationToken);
    Task<UserDto> GetUserByEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken);
    Task<UserDto> GetUserByUuid(Guid uuid, CancellationToken cancellationToken);
    Task ConfirmEmailAsync(Guid emailConfirmationToken, CancellationToken cancellationToken);
    Task RequestPasswordResetAsync(string email, CancellationToken cancellationToken);
    Task ResetPasswordAsync(Guid token, string newPassword, CancellationToken cancellationToken);
}