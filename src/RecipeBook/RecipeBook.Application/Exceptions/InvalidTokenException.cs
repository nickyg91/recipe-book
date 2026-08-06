namespace RecipeBook.Application.Exceptions;

public sealed class InvalidTokenException(string message) : Exception(message);
