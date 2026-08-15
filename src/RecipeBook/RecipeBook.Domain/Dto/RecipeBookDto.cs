namespace RecipeBook.Domain.Dto;

public record RecipeBookDto
{
    public required string Title { get; init; }
    public Guid Uuid { get; init; }
}