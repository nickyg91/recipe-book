namespace RecipeBook.Api.Models;

public record ResetPasswordRequest(Guid Token, string Password, string ConfirmPassword);
