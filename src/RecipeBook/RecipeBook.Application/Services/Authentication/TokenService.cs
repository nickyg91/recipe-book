using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RecipeBook.Application.Dto;
using RecipeBook.Domain.Authentication;
using RecipeBook.Infrastructure.Cache;

namespace RecipeBook.Application.Services.Authentication
{
    public class TokenService(TokenSettings settings, IRedisCache cache) : ITokenService
    {
        private const int AccessTokenMinutes = 60; // 1 hour
        private const int RefreshTokenMinutes = 24 * 60; // 24 hours

        public JwtToken CreateToken(UserDto user)
        {
            var tokenHandler = new JsonWebTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(settings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = settings.Audience,
                Issuer = settings.Issuer,
                Expires = DateTime.UtcNow.AddMinutes(AccessTokenMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity
                ([
                    new Claim(ClaimTypes.NameIdentifier, user.Uuid.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                ])
            };

            string? token = tokenHandler.CreateToken(tokenDescriptor);
            
            var refreshToken = Guid.NewGuid().ToString();
            
            Task.Run(() => cache.SetAsync(refreshToken, user, RefreshTokenMinutes));
            
            return new JwtToken(token, refreshToken, AccessTokenMinutes * 60, RefreshTokenMinutes * 60);
        }

        public async Task<JwtToken?> RefreshToken(string refreshToken)
        {
            UserDto? user = await cache.GetAsync<UserDto>(refreshToken);

            if (user == null)
            {
                return null;
            }

            await cache.RemoveAsync(refreshToken);
            return CreateToken(user);
        }
    }
}
