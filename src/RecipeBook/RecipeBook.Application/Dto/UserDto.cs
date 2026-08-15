using System.ComponentModel.DataAnnotations;

namespace RecipeBook.Application.Dto;

public record UserDto
{
    public int Id { get; init; }
    [MinLength(8), MaxLength(128)]
    public required string Username { get; init; }
    [EmailAddress]
    public required string Email { get; init; }
    [RegularExpression(@"^^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$$")]
    public string? Password { get; init; }
    public Guid Uuid { get; init; }
}