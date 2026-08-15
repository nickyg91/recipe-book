using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Entities.Recipes;

namespace RecipeBook.Infrastructure.Database.Context.RecipeBook.EntityTypeConfigurations;

public class RecipeStepIngredientEntityTypeConfiguration : BaseEntityTypeConfiguration<RecipeStepIngredientEntity>
{
    protected override string TableName => "recipe_step_ingredient";
    public override void Configure(EntityTypeBuilder<RecipeStepIngredientEntity> builder)
    {
        builder.Property(x => x.RecipeId)
            .HasColumnName("recipe_id");
        
        builder.Property(x => x.IngredientId)
            .HasColumnName("ingredient_id");
        
        builder.Property(x => x.RecipeStepId)
            .HasColumnName("recipe_step_id");
        
        builder
            .HasOne(rsi => rsi.RecipeStep)
            .WithMany(rs => rs.RecipeStepIngredients)
            .HasForeignKey(rsi => rsi.RecipeStepId)
            .HasConstraintName("fk_recipe_step_ingredient_recipe_step");
        
        base.Configure(builder);
    }
}