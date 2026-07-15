using System.Text.Json;
using System.Text.Json.Serialization;
using RecipeBook.Api.Endpoints;
using RecipeBook.Application.Extensions;
using RecipeBook.Domain.Authentication;
using RecipeBook.Domain.Email;
using RecipeBook.Infrastructure.Database.Context.RecipeBook;

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

string tokenSecret = builder.Configuration["TOKEN_SECRET"] ?? throw new ArgumentException("TOKEN_SECRET is not set");
string tokenIssuer = builder.Configuration["TOKEN_ISSUER"] ?? throw new ArgumentException("TOKEN_ISSUER is not set");
string tokenAudience = builder.Configuration["TOKEN_AUDIENCE"] ?? throw new ArgumentException("TOKEN_AUDIENCE is not set");

TokenSettings tokenSettings = new()
{
    Secret = tokenSecret,
    Issuer = tokenIssuer,
    Audience = tokenAudience
};

builder.Services.AddSingleton(tokenSettings);

string smtpHost = builder.Configuration["SMTP_HOST"] ?? throw new ArgumentException("SMTP_HOST is not set");
string smtpPort = builder.Configuration["SMTP_PORT"] ?? throw new ArgumentException("SMTP_PORT is not set");;

SmtpSettings smtpSettings = new(smtpHost, int.Parse(smtpPort));

builder.Services.AddSmtpClient(smtpSettings);

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
