using RecipeBook.Application.Dto;

namespace RecipeBook.Application.Services.Authentication;

public interface ITokenService
{
    /// <summary>
    /// Creates a bearer token and a refresh token.
    /// </summary>
    /// <returns>A tuple containing the access token (JWT) and the refresh token.</returns>
    public JwtToken CreateToken(UserDto user);
}
