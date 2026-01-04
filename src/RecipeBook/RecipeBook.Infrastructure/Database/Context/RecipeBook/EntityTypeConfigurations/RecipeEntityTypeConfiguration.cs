
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeBook.Domain.Entities;

namespace RecipeBook.Infrastructure.Database.Context.RecipeBook.EntityTypeConfigurations;

public class RecipeEntityTypeConfiguration : BaseEntityTypeConfiguration<RecipeEntity>
{
    protected override string  TableName => "recipe";
    public override void Configure(EntityTypeBuilder<RecipeEntity> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .IsUnicode()
            .HasColumnName("name")
            .HasMaxLength(256);

        builder.Property(x => x.Description)
            .IsRequired()
            .IsUnicode()
            .HasColumnName("description")
            .HasMaxLength(512);

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder.Property(x => x.EstimatedTime)
            .IsRequired()
            .HasColumnName("estimated_time");
        
        builder.Property(x => x.Image)
            .HasColumnName("image")
            .HasColumnType("bytea")
            //5 MB
            .HasMaxLength(5_242_880);

        builder.Property(x => x.IsPrivate)
            .HasDefaultValueSql("false")
            .HasColumnName("is_private");
        
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Recipes)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_recipe_user");

        builder.HasMany(x => x.RecipeSteps)
            .WithOne(x => x.Recipe)
            .HasForeignKey(x => x.RecipeId)
            .HasConstraintName("fk_recipe_step_recipe");
        
        builder.HasMany(x => x.RecipeIngredients)
            .WithOne(x => x.Recipe)
            .HasForeignKey(x => x.RecipeId)
            .HasConstraintName("fk_recipe_ingredient");
        
        base.Configure(builder);
    }
}