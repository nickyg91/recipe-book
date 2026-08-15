using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RecipeBook.Infrastructure.Database.Context.RecipeBook;

public class RecipeBookDesignTimeDbContext : IDesignTimeDbContextFactory<RecipeBookDbContext>
{
    public RecipeBookDbContext CreateDbContext(string[]? args)
    {
        // EF Core passes the --connection value as the first argument
        string connectionString = args is { Length: > 0 }
            ? args[0]
            : "Host=localhost;Port=5432;Database=recipe-book;Username=postgres;Password=dev_postgres";

        var optionsBuilder = new DbContextOptionsBuilder<RecipeBookDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new RecipeBookDbContext(optionsBuilder.Options);
    }
}