using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using RecipeBook.Api.Endpoints;
using RecipeBook.Application.Extensions;
using RecipeBook.Application.Services.Email;
using RecipeBook.Domain.Authentication;
using RecipeBook.Domain.Email;
using RecipeBook.Infrastructure.Database.Context.RecipeBook;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RecipeBook.Infrastructure.Cache;

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);


builder.Services.AddProblemDetails();
builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

string tokenSecret = builder.Configuration["TOKEN_SECRET"] ?? throw new ArgumentException("TOKEN_SECRET is not set");
string tokenIssuer = builder.Configuration["TOKEN_ISSUER"] ?? throw new ArgumentException("TOKEN_ISSUER is not set");
string tokenAudience = builder.Configuration["TOKEN_AUDIENCE"] ?? throw new ArgumentException("TOKEN_AUDIENCE is not set");

string redisConnectionString = builder.Configuration.GetConnectionString("Redis") ??  throw new ArgumentException("Redis connection string is not set");

TokenSettings tokenSettings = new()
{
    Secret = tokenSecret,
    Issuer = tokenIssuer,
    Audience = tokenAudience
};

builder.Services.AddSingleton(tokenSettings);
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddTokenService();
builder.Services.AddLogging();
builder.Services.AddRedisCache(redisConnectionString);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = tokenSettings.Issuer,
        ValidAudience = tokenSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Secret))
    };
});

builder.Services.AddAuthorization();

SmtpSettings smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>() ?? throw new ArgumentException("SmtpSettings is not set");

string frontendUrl = builder.Configuration["FrontendUrl"] ?? throw new ArgumentException("FrontendUrl is not set");

builder.Services.AddKeyedSingleton("frontendUrl", frontendUrl);
builder.Services.AddSingleton(smtpSettings);

builder.Services.AddUserServices();

builder.AddNpgsqlDbContext<RecipeBookDbContext>("RecipeBook");
builder.Services.AddCors();

WebApplication app = builder.Build();


if (builder.Environment.IsDevelopment())
{
    app.UseCors(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
}
else
{
    app.UseCors(policy =>
    {
        policy.WithOrigins("recipes.nickganter.dev")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
}

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();

app.Run();
