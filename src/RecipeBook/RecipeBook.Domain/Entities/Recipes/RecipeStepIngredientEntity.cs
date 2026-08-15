namespace RecipeBook.Domain.Entities.Recipes;

public class RecipeStepIngredientEntity : BaseEntity
{
    public int RecipeId { get; set; }
    public int RecipeStepId { get; set; }
    public int IngredientId { get; set; }
    public RecipeStepEntity? RecipeStep { get; set; }
    public RecipeEntity? Recipe { get; set; }
    public RecipeIngredientEntity? Ingredient { get; set; }
}