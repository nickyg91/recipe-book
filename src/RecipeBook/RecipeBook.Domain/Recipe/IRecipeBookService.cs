using RecipeBook.Domain.Dto;

namespace RecipeBook.Domain.Recipe;

public interface IRecipeBookService
{
    Task<List<RecipeBookDto>> GetRecipeBooksForUser(Guid userId);
    Task<RecipeBookDto> GetRecipeBook(Guid recipeBookId);
    Task DeleteRecipeBook(Guid recipeBookId);
    Task<RecipeBookDto> CreateRecipeBook(Guid userId, RecipeBookDto recipeBookDto);
    Task<RecipeBookDto> UpdateRecipeBook(Guid recipeBookId, RecipeBookDto recipeBookDto);
}