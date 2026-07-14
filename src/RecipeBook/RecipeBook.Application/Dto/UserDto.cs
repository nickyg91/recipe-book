namespace RecipeBook.Application.Dto;

public record UserDto
{
    public int Id { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public string? Password { get; init; } = string.Empty;
}