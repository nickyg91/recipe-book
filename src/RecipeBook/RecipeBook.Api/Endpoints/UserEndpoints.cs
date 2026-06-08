using RecipeBook.Application.Domain.Services.User;
using RecipeBook.Application.Dto;
using RecipeBook.Application.Exceptions;

namespace RecipeBook.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/users").WithTags("Users");
        group.MapPost("sign-up", async (
            UserDto user, 
            CancellationToken cancellationToken, 
            IUserService userService, 
            ILogger logger) =>
        {
            try
            {
                await userService.CreateUserAccountAsync(user, cancellationToken);
                return Results.Ok();
            }
            catch (UserExistsException e)
            {
                logger.LogError(e, "{message}", e.Message);
                return Results.Ok();
            }
        });
    }
}