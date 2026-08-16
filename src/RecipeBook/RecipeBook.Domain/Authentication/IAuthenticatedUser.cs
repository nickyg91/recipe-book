namespace RecipeBook.Domain.Authentication;

public interface IAuthenticatedUser
{
    Guid Uuid { get; }
    string Email { get; }
    string Name { get; }
}