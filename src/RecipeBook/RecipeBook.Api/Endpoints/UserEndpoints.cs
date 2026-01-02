namespace RecipeBook.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/users").WithTags("Users");

        group.MapGet("/", () => Results.Ok());
    }
}