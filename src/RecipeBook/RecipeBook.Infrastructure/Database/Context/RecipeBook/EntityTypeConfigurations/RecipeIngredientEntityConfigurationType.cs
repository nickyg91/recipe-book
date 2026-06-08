using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Entities.Recipes;

namespace RecipeBook.Infrastructure.Database.Context.RecipeBook.EntityTypeConfigurations;

public class RecipeIngredientEntityConfigurationType : BaseEntityTypeConfiguration<RecipeIngredientEntity>
{
    protected override string TableName => "recipe_ingredient";
    public override void Configure(EntityTypeBuilder<RecipeIngredientEntity> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .IsUnicode()
            .HasColumnName("name")
            .HasMaxLength(256);
        
        builder.Property(x => x.MeasurementType)
            .IsRequired()
            .HasColumnName("measurement_type");
        
        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasColumnName("quantity");
        
        builder.Property(x => x.Description)
            .IsUnicode()
            .IsRequired(false)
            .HasColumnName("description")
            .HasMaxLength(512);

        builder.Property(x => x.RecipeId)
            .HasColumnName("recipe_id");
        
        builder.HasOne(x => x.Recipe)
            .WithMany(x => x.RecipeIngredients)
            .HasForeignKey(x => x.RecipeId)
            .HasConstraintName("fk_recipe_ingredient_recipe");
        
        base.Configure(builder);
    }
}