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
}