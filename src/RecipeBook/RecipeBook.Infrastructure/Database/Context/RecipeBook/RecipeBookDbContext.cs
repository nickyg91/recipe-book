using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;

namespace RecipeBook.Infrastructure.Database.Context.RecipeBook;

public class RecipeBookDbContext : DbContext
{
    public RecipeBookDbContext(DbContextOptions<RecipeBookDbContext> options) : base(options)
    {
        
    }
    
    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<RecipeEntity> Recipes { get; set; }
    public virtual DbSet<RecipeIngredientEntity> RecipeIngredients { get; set; }
    public virtual DbSet<RecipeStepEntity> RecipeSteps { get; set; }
    public virtual DbSet<RecipeStepIngredientEntity> RecipeStepIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeBookDbContext).Assembly);
    }
}