using System.Security.Claims;

namespace RecipeBook.Domain.Authentication;

public class AuthenticatedUser(ClaimsPrincipal identity) : IAuthenticatedUser
{
    public Guid Uuid => new (identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value);
    public string Email => identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
    public string Name => identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
}