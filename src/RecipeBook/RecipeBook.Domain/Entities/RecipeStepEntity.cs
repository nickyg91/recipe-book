namespace RecipeBook.Domain.Entities;

public class RecipeStepEntity : BaseEntity
{
    public int RecipeId { get; set; }
    public required string StepDirections { get; set; }
    public ICollection<RecipeStepIngredientEntity> RecipeStepIngredients { get; set; } = [];
    public RecipeEntity? Recipe { get; set; }
}