namespace RecipeBook.Domain.Dto;

public class RecipeDto
{
    public Guid Uuid { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public short EstimatedTime { get; init; }
    public byte[]? Image { get; init; }
    public bool IsPrivate { get; init; }
    public List<RecipeStepDto> Steps { get; init; } = [];
    public List<RecipeIngredientDto> Ingredients { get; init; } = [];
}