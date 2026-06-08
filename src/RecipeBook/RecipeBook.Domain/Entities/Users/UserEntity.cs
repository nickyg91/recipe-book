using RecipeBook.Domain.Entities.Recipes;

namespace RecipeBook.Domain.Entities.Users;

public class UserEntity : BaseEntity
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    //public bool IsEmailConfirmed { get; set; }
    //public Guid? EmailConfirmationToken { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public ICollection<RecipeEntity> Recipes { get; set; } = [];
}