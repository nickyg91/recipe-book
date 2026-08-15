using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Entities.Recipes;

namespace RecipeBook.Infrastructure.Database.Context.RecipeBook.EntityTypeConfigurations;

public class RecipeBookEntityTypeConfiguration : BaseEntityTypeConfiguration<RecipeBookEntity>
{
    protected override string TableName => "recipe_book";
    public override void Configure(EntityTypeBuilder<RecipeBookEntity> builder)
    {
        builder.Property(x => x.Title)
            .IsRequired()
            .IsUnicode()
            .HasColumnName("title")
            .HasMaxLength(256);

        builder.Property(x => x.UserId)
            .HasColumnName("user_id");

        builder.Property(x => x.Uuid)
            .HasColumnName("uuid")
            .HasDefaultValueSql("uuidv4()");
        
        builder
            .HasMany(x => x.Recipes)
            .WithOne(x => x.RecipeBook)
            .HasConstraintName("fk_recipe_book_recipe");

        builder
            .HasIndex(x => x.Uuid)
            .IsUnique()
            .HasDatabaseName("ix_recipe_book_uuid");
        
        base.Configure(builder);
    }
}
