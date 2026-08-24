using RecipeBook.Domain.Authentication;
using RecipeBook.Domain.Dto;
using RecipeBook.Domain.Recipe;

namespace RecipeBook.Api.Endpoints;

public static class RecipeBookEndpoints
{
    public static void MapRecipeBookEndpoints(this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder recipeBookGroup = endpoints
            .MapGroup("/api/recipe-books")
            .WithTags("RecipeBooks");
        
        recipeBookGroup.MapPost("create", async (RecipeBookDto recipeBook, IAuthenticatedUser user, IRecipeBookService recipeBookService) 
            => await recipeBookService.CreateRecipeBook(user.Uuid, recipeBook)).RequireAuthorization();
        
        recipeBookGroup.MapGet("", async (CancellationToken cancellationToken, IAuthenticatedUser user, IRecipeBookService recipeBookService) 
            => await recipeBookService.GetRecipeBooksForUser(user.Uuid, cancellationToken)).RequireAuthorization();

        endpoints.MapGet("/api/tags/all", async (IRecipeBookService service) 
            => Results.Ok(await service.GetAllTags())).WithTags("Tags").RequireAuthorization();
    }
}