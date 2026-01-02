using System.Text.Json;
using System.Text.Json.Serialization;
using RecipeBook.Application.Extensions;
using RecipeBook.Infrastructure.Database.Context.RecipeBook;

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddUserServices();

builder.AddNpgsqlDbContext<RecipeBookDbContext>("recipe-book");

WebApplication app = builder.Build();

app.Run();