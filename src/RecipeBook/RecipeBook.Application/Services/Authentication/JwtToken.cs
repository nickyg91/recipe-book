namespace RecipeBook.Application.Services.Authentication;

public record JwtToken(string Token, string RefreshToken, int ExpiresInSeconds, int RefreshExpiresInSeconds);
