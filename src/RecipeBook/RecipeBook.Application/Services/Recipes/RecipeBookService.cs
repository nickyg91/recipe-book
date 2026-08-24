using Microsoft.EntityFrameworkCore;
using RecipeBook.Application.Mappers;
using RecipeBook.Domain.Dto;
using RecipeBook.Domain.Entities.Recipes;
using RecipeBook.Domain.Recipe;
using RecipeBook.Infrastructure.Cache;
using RecipeBook.Infrastructure.Database.Context.RecipeBook;
using StackExchange.Redis;

namespace RecipeBook.Application.Services.Recipes;

internal sealed class RecipeBookService(
    RecipeBookDbContext dbContext,
    IRedisCache redisCache,
    ConnectionMultiplexer multiplexer
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

        // Store per-book tags for potential future removal needs (JSON format)
        string tagsKey = $"recipe-book:{entity.Uuid}:tags";
        foreach (string tag in recipeBookDto.Tags)
        {
            await redisCache.SetAsync(tagsKey, tag.ToLowerInvariant(), null);
        }

        // Add to global unique tags set using raw Redis pipeline — no JSON serialization overhead.
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

        // Add to global unique tags set using raw Redis pipeline — no JSON serialization overhead.
        string allTagsKey = "all-unique-tags";
        foreach (string tag in recipeBookDto.Tags)
        {
            await redisCache.AddToSetAsync(allTagsKey, tag.ToLowerInvariant());
        }

        return _mapper.ToRecipeBookDto(entity);
    }

    public async Task<HashSet<string>> GetAllTags()
    {
        // O(1) — single Redis GET command on a SET.
        // HashGetAll reads all members of the SET (stored internally as hash keys).
        // No JSON serialization overhead — values are already plain strings from Redis.
        var entries = await redisCache.GetAsync<Dictionary<string, string>>("all-unique-tags") ?? [];

        HashSet<string> result = [];
        foreach (var kvp in entries)
        {
            result.Add(kvp.Key); // The key IS the tag string stored by SetAsync
        }
        return result;
    }
}
