namespace RecipeBook.Domain.Entities;

public class RecipeStepIngredient : BaseEntity
{
    public int RecipeId { get; set; }
    public int RecipeStepId { get; set; }
    public int IngredientId { get; set; }
    public RecipeStep? RecipeStep { get; set; }
    public Recipe? Recipe { get; set; }
    public RecipeIngredient? Ingredient { get; set; }
}