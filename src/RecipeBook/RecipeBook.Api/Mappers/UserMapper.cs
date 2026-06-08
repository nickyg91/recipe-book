using RecipeBook.Api.Models;
using RecipeBook.Application.Dto;
using Riok.Mapperly.Abstractions;

namespace RecipeBook.Api.Mappers;

[Mapper]
public partial class UserMapper
{
    [MapperIgnoreTarget(nameof(UserDto.Id))]
    public partial UserDto ToUserDto(CreateUserRequest createUser);
}