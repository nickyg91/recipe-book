namespace RecipeBook.Domain.Dto;

public record RecipeBookDto
{
    public required string Title { get; init; }
    public Guid Uuid { get; init; }
    public required string[] Tags { get; init; } = [];
    public List<RecipeDto> Recipes { get; init; } = [];
}