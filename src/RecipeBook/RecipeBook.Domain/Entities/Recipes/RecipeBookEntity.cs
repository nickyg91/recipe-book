using RecipeBook.Domain.Entities.Users;

namespace RecipeBook.Domain.Entities.Recipes;

public class RecipeBookEntity : BaseEntity
{
    public required string Title { get; set; }
    public int UserId { get; set; }
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public UserEntity? User { get; set; }
    public HashSet<RecipeEntity> Recipes { get; set; } = [];
    public HashSet<string> Tags { get; set; } = [];
}