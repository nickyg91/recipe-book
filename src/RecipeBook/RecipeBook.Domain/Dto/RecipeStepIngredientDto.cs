using RecipeBook.Domain.Enums;

namespace RecipeBook.Domain.Dto;

public record RecipeStepIngredientDto
{
    public int RecipeStepId { get; init; }
    public int IngredientId { get; init; }
    public required string IngredientName { get; init; }
    public int Quantity { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public MeasurementType MeasurementType { get; set; }
}