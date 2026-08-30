using Microsoft.EntityFrameworkCore;
using RecipeBook.Application.Exceptions;
using RecipeBook.Application.Mappers;
using RecipeBook.Domain.Dto;
using RecipeBook.Domain.Entities.Recipes;
using RecipeBook.Domain.Recipe;
using RecipeBook.Infrastructure.Cache;
using RecipeBook.Infrastructure.Database.Context.RecipeBook;

namespace RecipeBook.Application.Services.Recipes;

internal sealed class RecipeBookService(
    RecipeBookDbContext dbContext,
    IRedisCache redisCache
) : IRecipeBookService
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
        RecipeBookEntity entity = await dbContext.RecipeBooks
                                      .Include(x => x.Recipes)
                                      .ThenInclude(x => x.RecipeSteps)
                                      .ThenInclude(x => x.RecipeStepIngredients)
                                      .Include(x => x.Recipes)
                                      .ThenInclude(x => x.RecipeIngredients)
                                      .SingleOrDefaultAsync(x => x.Uuid == recipeBookId, cancellationToken) 
                                  ?? throw new EntityNotFoundException("Recipe book not found.");
        
        return _mapper.ToRecipeBookDto(entity);
    }

    public async Task DeleteRecipeBook(Guid recipeBookId)
    {
        await dbContext.RecipeBooks.Where(x => x.Uuid == recipeBookId).ExecuteDeleteAsync();
    }

    public async Task<RecipeBookDto> CreateRecipeBook(Guid userId, RecipeBookDto recipeBookDto)
    {
        RecipeBookEntity entity = _mapper.ToRecipeBookEntity(recipeBookDto);
        await dbContext.RecipeBooks.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        // Store per-book tags for potential future removal needs (JSON format)
        string tagsKey = $"recipe-book:{entity.Uuid}:tags";
        foreach (string tag in recipeBookDto.Tags)
        {
            await redisCache.SetAsync(tagsKey, tag.ToLowerInvariant(), null);
        }

        string allTagsKey = "all-unique-tags";
        foreach (string tag in recipeBookDto.Tags)
        {
            await redisCache.AddToSetAsync(allTagsKey, tag.ToLowerInvariant());
        }

        return _mapper.ToRecipeBookDto(entity);
    }

    public async Task<RecipeBookDto> UpdateRecipeBook(Guid recipeBookId, RecipeBookDto recipeBookDto)
    {
        RecipeBookEntity? entity = await dbContext.RecipeBooks.FindAsync(recipeBookId);
        if (entity == null) throw new Exception("Recipe book not found.");

        _mapper.Update(entity, recipeBookDto);
        await dbContext.SaveChangesAsync();

        string tagsKey = $"recipe-book:{entity.Uuid}:tags";
        foreach (string tag in recipeBookDto.Tags)
        {
            await redisCache.SetAsync(tagsKey, tag.ToLowerInvariant(), null);
        }

        string allTagsKey = "all-unique-tags";
        foreach (string tag in recipeBookDto.Tags)
        {
            await redisCache.AddToSetAsync(allTagsKey, tag.ToLowerInvariant());
        }

        return _mapper.ToRecipeBookDto(entity);
    }

    public async Task<HashSet<string>> GetAllTags()
    {
        var entries = await redisCache.GetAsync<Dictionary<string, string>>("all-unique-tags") ?? [];

        HashSet<string> result = [];
        foreach (var kvp in entries)
        {
            result.Add(kvp.Key);
        }
        return result;
    }
}
