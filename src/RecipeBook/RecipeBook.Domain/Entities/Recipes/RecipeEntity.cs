using RecipeBook.Domain.Entities.Users;

namespace RecipeBook.Domain.Entities.Recipes;

public class RecipeEntity : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public short EstimatedTime { get; set; }
    public int RecipeBookId { get; set; }
    public byte[]? Image { get; set; }
    public RecipeBookEntity? RecipeBook { get; set; }
    public bool IsPrivate { get; set; }
    public ICollection<RecipeIngredientEntity> RecipeIngredients { get; set; } = [];
    public ICollection<RecipeStepEntity> RecipeSteps { get; set; } = [];
}