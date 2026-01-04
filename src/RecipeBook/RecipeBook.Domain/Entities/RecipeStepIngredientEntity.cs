namespace RecipeBook.Domain.Entities;

public class RecipeStepIngredientEntity : BaseEntity
{
    public int RecipeId { get; set; }
    public int RecipeStepId { get; set; }
    public int IngredientId { get; set; }
    public RecipeStepEntity? RecipeStep { get; set; }
    public RecipeEntity? Recipe { get; set; }
    public RecipeIngredientEntity? Ingredient { get; set; }
}