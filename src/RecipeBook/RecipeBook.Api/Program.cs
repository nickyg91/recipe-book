using System.Text.Json;
using System.Text.Json.Serialization;
using RecipeBook.Api.Endpoints;
using RecipeBook.Application.Extensions;
using RecipeBook.Infrastructure.Database.Context.RecipeBook;

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddUserServices();

builder.AddNpgsqlDbContext<RecipeBookDbContext>("recipe-book");

WebApplication app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseCors(policy =>
    {
        policy.AllowAnyOrigin();
    });
}
else
{
    app.UseCors(policy =>
    {
        policy.WithOrigins("cookbook.nickganter.dev");
    });
}

app.MapUserEndpoints();

app.Run();