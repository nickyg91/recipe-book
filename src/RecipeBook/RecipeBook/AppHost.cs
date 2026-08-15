using Microsoft.Extensions.Configuration;
using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
string? connectionString = builder.Configuration.GetConnectionString("RecipeBook");

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("RecipeBook connection string is empty");
}

IResourceBuilder<IResourceWithConnectionString> postgresDb = builder.AddConnectionString("RecipeBook");
IResourceBuilder<IResourceWithConnectionString> cache = builder.AddRedis("Redis");

builder.AddProject<RecipeBook_Api>("recipebook-api")
    .WithReference(postgresDb)
    .WithReference(cache);

builder.Build().Run();