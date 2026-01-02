using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;

namespace RecipeBook.Infrastructure.Database.Context.RecipeBook;

public class RecipeBookDbContext : DbContext
{
    public RecipeBookDbContext(DbContextOptions<RecipeBookDbContext> options) : base(options)
    {
        
    }
    
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Recipe> Recipes { get; set; }
    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public virtual DbSet<RecipeStep> RecipeSteps { get; set; }
    public virtual DbSet<RecipeStepIngredient> RecipeStepIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeBookDbContext).Assembly);
    }
}