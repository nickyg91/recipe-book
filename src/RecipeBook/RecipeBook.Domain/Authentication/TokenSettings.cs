namespace RecipeBook.Domain.Authentication;

public record TokenSettings
{
    public required string Audience { get; init; }
    public required  string Issuer { get; init; }
    public required  string Secret { get; init; }
}