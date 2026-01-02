namespace RecipeBook.Domain.Entities;

public class RecipeStep : BaseEntity
{
    public int RecipeId { get; set; }
    public required string StepDirections { get; set; }
    public ICollection<RecipeStepIngredient> RecipeStepIngredients { get; set; } = [];
    public Recipe? Recipe { get; set; }
}