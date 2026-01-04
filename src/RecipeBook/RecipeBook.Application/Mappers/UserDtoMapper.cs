using RecipeBook.Application.Domain.Dto;
using RecipeBook.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace RecipeBook.Application.Mappers;

[Mapper(ThrowOnMappingNullMismatch = false)]
internal partial class UserDtoMapper
{
    [MapperIgnoreSource(nameof(UserEntity.Recipes))]
    [MapperIgnoreSource(nameof(UserEntity.CreatedAtUtc))]
    public partial UserDto ToUserDto(UserEntity user);
    
    [MapperIgnoreTarget(nameof(UserEntity.Recipes))]
    [MapperIgnoreTarget(nameof(UserEntity.CreatedAtUtc))]
    public partial UserEntity ToUserEntity(UserDto userDto);
}