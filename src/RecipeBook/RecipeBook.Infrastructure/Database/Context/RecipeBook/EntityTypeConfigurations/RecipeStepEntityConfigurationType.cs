using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Entities.Recipes;

namespace RecipeBook.Infrastructure.Database.Context.RecipeBook.EntityTypeConfigurations;

public class RecipeStepEntityConfigurationType : BaseEntityTypeConfiguration<RecipeStepEntity>
{
    protected override string TableName => "recipe_step";
    public override void Configure(EntityTypeBuilder<RecipeStepEntity> builder)
    {
        builder.Property(x => x.StepDirections)
            .IsRequired()
            .IsUnicode()
            .HasColumnName("step_directions")
            .HasMaxLength(512);

        builder.Property(x => x.RecipeId)
            .HasColumnName("recipe_id");
        
        builder
            .HasMany(x => x.RecipeStepIngredients)
            .WithOne(x => x.RecipeStep)
            .HasConstraintName("fk_recipe_step_recipe_step_ingredient")
            .HasForeignKey(x => x.Id);
        
        base.Configure(builder);
    }
}