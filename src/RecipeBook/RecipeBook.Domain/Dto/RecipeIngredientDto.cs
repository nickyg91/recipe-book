using RecipeBook.Domain.Enums;

namespace RecipeBook.Domain.Dto;

public record RecipeIngredientDto
{
    public Guid Uuid { get; set; }
    public MeasurementType MeasurementType { get; set; }
    public int Quantity { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}