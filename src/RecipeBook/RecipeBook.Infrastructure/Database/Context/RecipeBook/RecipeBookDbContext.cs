using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities.Recipes;
using RecipeBook.Domain.Entities.Users;

namespace RecipeBook.Infrastructure.Database.Context.RecipeBook;

public class RecipeBookDbContext(DbContextOptions<RecipeBookDbContext> options) : DbContext(options)
{
    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<RecipeEntity> Recipes { get; set; }
    public virtual DbSet<RecipeIngredientEntity> RecipeIngredients { get; set; }
    public virtual DbSet<RecipeStepEntity> RecipeSteps { get; set; }
    public virtual DbSet<RecipeStepIngredientEntity> RecipeStepIngredients { get; set; }
    public virtual DbSet<RecipeBookEntity> RecipeBooks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeBookDbContext).Assembly);
    }
}