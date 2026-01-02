using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RecipeBook.Infrastructure.Database.Context.RecipeBook;

public class RecipeBookDesignTimeDbContext : IDesignTimeDbContextFactory<RecipeBookDbContext>
{
    public RecipeBookDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RecipeBookDbContext>();
        optionsBuilder.UseNpgsql(args[0]);

        return new RecipeBookDbContext(optionsBuilder.Options);
    }
}