using System.ComponentModel.DataAnnotations;

namespace RecipeBook.Api.Models;

public record CreateUserRequest(
    [Required] string Username,
    [Required,
     RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{12,}$",
         ErrorMessage =
             "Password must be at least 12 characters, one lowercase letter, one uppercase letter, one digit, and one special character.")]
    string Password,
    [Required, EmailAddress] string Email,
    DateOnly? DateOfBirth);