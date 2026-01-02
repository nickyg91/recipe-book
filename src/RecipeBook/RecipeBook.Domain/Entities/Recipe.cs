namespace RecipeBook.Domain.Entities;

public class Recipe : BaseEntity
{
    public int UserId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public short EstimatedTime { get; set; }
    public byte[]? Image { get; set; }
    public User? User { get; set; }
    public bool IsPrivate { get; set; }
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = [];
    public ICollection<RecipeStep> RecipeSteps { get; set; } = [];
}