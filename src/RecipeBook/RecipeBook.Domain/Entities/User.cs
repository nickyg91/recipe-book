namespace RecipeBook.Domain.Entities;

public class User : BaseEntity
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public ICollection<Recipe> Recipes { get; set; } = [];
}