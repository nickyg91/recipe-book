using RecipeBook.Domain.Enums;

namespace RecipeBook.Domain.Entities;

public class RecipeIngredient : BaseEntity
{
    public int RecipeId { get; set; }
    public MeasurementType MeasurementType { get; set; }
    public int Quantity { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Recipe? Recipe { get; set; }
}