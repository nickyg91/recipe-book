using Microsoft.EntityFrameworkCore;
using RecipeBook.Application.Mappers;
using RecipeBook.Domain.Dto;
using RecipeBook.Domain.Entities.Recipes;
using RecipeBook.Domain.Recipe;
using RecipeBook.Infrastructure.Database.Context.RecipeBook;

namespace RecipeBook.Application.Services.Recipes;

internal sealed class RecipeBookService(RecipeBookDbContext dbContext) : IRecipeBookService
{
    private readonly RecipeBookMapper _mapper = new();
    public async Task<List<RecipeBookDto>> GetRecipeBooksForUser(Guid userId, CancellationToken cancellationToken)
    {
        List<RecipeBookEntity> recipeBooks = await dbContext.RecipeBooks
            .Include(x => x.User)
            .Where(x => x.Uuid == userId)
            .ToListAsync(cancellationToken);
        return [.. recipeBooks.Select(x => _mapper.ToRecipeBookDto(x))];
    }

    public async Task<RecipeBookDto> GetRecipeBook(Guid recipeBookId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteRecipeBook(Guid recipeBookId)
    {
        throw new NotImplementedException();
    }

    public async Task<RecipeBookDto> CreateRecipeBook(Guid userId, RecipeBookDto recipeBookDto)
    {
        RecipeBookEntity entity = _mapper.ToRecipeBookEntity(recipeBookDto);
        await dbContext.RecipeBooks.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return _mapper.ToRecipeBookDto(entity);
    }

    public async Task<RecipeBookDto> UpdateRecipeBook(Guid recipeBookId, RecipeBookDto recipeBookDto)
    {
        throw new NotImplementedException();
    }
}