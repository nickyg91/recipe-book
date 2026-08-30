namespace RecipeBook.Domain.Dto;

public record RecipeStepDto
{
    public required string StepDirections { get; init; }
    public List<RecipeStepIngredientDto> Ingredients { get; init; } = [];
}