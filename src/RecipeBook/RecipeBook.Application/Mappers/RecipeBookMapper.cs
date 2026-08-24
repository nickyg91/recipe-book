using RecipeBook.Domain.Dto;
using RecipeBook.Domain.Entities.Recipes;
using Riok.Mapperly.Abstractions;

namespace RecipeBook.Application.Mappers;

[Mapper]
public partial class RecipeBookMapper
{
    [MapperIgnoreSource(nameof(RecipeBookEntity.User))]
    [MapperIgnoreSource(nameof(RecipeBookEntity.CreatedAtUtc))]
    [MapperIgnoreSource(nameof(RecipeBookEntity.Id))]
    public partial RecipeBookDto ToRecipeBookDto(RecipeBookEntity recipeBook);
    
    public partial RecipeBookEntity ToRecipeBookEntity(RecipeBookDto recipeBookDto);

    private static string[] MapTagsToArray(HashSet<string> tags) => [.. tags];
    private static HashSet<string> MapTagArrayToSet(string[]? tags) => tags == null ? [] : new HashSet<string>(tags);

    public partial void Update(RecipeBookEntity recipeBook, RecipeBookDto dto);
}