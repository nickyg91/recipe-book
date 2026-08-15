using RecipeBook.Application.Dto;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Entities.Users;
using Riok.Mapperly.Abstractions;

namespace RecipeBook.Application.Mappers;

[Mapper(ThrowOnMappingNullMismatch = false)]
internal partial class UserDtoMapper
{
    [MapperIgnoreSource(nameof(UserEntity.Recipes))]
    [MapperIgnoreSource(nameof(UserEntity.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(UserEntity.Password))]
    public partial UserDto ToUserDto(UserEntity user);
    
    [MapperIgnoreTarget(nameof(UserEntity.Recipes))]
    [MapperIgnoreTarget(nameof(UserEntity.CreatedAtUtc))]
    public partial UserEntity ToUserEntity(UserDto userDto);
}