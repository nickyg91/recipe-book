using System.Collections.Generic;
using RecipeBook.Domain.Dto;

namespace RecipeBook.Domain.Recipe;

public interface IRecipeBookService
{
    Task<List<RecipeBookDto>> GetRecipeBooksForUser(Guid userId, CancellationToken cancellationToken);
    Task<RecipeBookDto> GetRecipeBook(Guid recipeBookId, CancellationToken cancellationToken);
    Task DeleteRecipeBook(Guid recipeBookId);
    Task<RecipeBookDto> CreateRecipeBook(Guid userId, RecipeBookDto recipeBookDto);
    Task<RecipeBookDto> UpdateRecipeBook(Guid recipeBookId, RecipeBookDto recipeBookDto);
    Task<HashSet<string>> GetAllTags();
}
