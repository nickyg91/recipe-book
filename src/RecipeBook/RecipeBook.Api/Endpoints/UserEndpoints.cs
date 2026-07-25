using Microsoft.AspNetCore.Mvc;
using RecipeBook.Api.Models;
using RecipeBook.Application.Dto;
using RecipeBook.Application.Exceptions;
using RecipeBook.Application.Services.Authentication;
using RecipeBook.Application.Services.User;

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
            ILogger<IUserService> logger) =>
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
        
        group.MapPut("confirm-account/{emailConfirmationToken}", async (
            Guid emailConfirmationToken, 
            CancellationToken cancellationToken, 
            IUserService userService) =>
        {
            try
            {
                await userService.ConfirmEmailAsync(emailConfirmationToken, cancellationToken);
                return Results.Ok();
            }
            catch (UserNotFoundException)
            {
                return Results.NotFound();
            }
        });

        group.MapPost("log-in", async (LogInRequest request, CancellationToken cancellationToken, IUserService userService, ITokenService tokenService, ILogger<IUserService> logger) =>
        {
            try
            {
                UserDto user =
                    await userService.GetUserByEmailAndPasswordAsync(request.Email, request.Password,
                        cancellationToken);
                
                JwtToken jwtToken = tokenService.CreateToken(user);
                return Results.Ok(jwtToken);
            }
            catch (UserNotFoundException)
            {
                logger.LogError("User {RequestEmail} not found.", request.Email);
                return Results.NotFound();
            }
            catch (AccountNotVerifiedException)
            {
                logger.LogError("User {RequestEmail} not verified.", request.Email);
                return Results.Unauthorized();
            }
            catch (Exception e)
            {
                logger.LogError("Error while logging in user {RequestEmail}: {ExceptionMessage}", request.Email, e.Message);
                return Results.InternalServerError(e);
            }
        });

        group.MapPut("token/{refreshToken}/refresh",
            async (string refreshToken, ITokenService tokenService) =>
            {
                JwtToken? token = await tokenService.RefreshToken(refreshToken);
                return token == null ? Results.Unauthorized() : Results.Ok(token);
            });
    }
}