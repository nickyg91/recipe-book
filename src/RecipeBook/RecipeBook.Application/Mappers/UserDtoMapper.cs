using RecipeBook.Application.Domain.Dto;
using RecipeBook.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace RecipeBook.Application.Mappers;

[Mapper(ThrowOnMappingNullMismatch = false)]
internal partial class UserDtoMapper
{
    [MapperIgnoreSource(nameof(User.Recipes))]
    [MapperIgnoreSource(nameof(User.CreatedAtUtc))]
    public partial UserDto ToUserDto(User user);
    
    [MapperIgnoreTarget(nameof(User.Recipes))]
    [MapperIgnoreTarget(nameof(User.CreatedAtUtc))]
    public partial User ToUserEntity(UserDto userDto);
}