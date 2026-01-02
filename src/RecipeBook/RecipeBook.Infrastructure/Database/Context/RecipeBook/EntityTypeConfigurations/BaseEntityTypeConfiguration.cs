using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeBook.Domain.Entities;

namespace RecipeBook.Infrastructure.Database.Context.RecipeBook.EntityTypeConfigurations;

public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    protected virtual string TableName { get; } = null!;

    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(TableName);
        builder.HasKey(x => x.Id).HasName($"pk_{TableName}_id");
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityColumn();
        
        builder
            .Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired()
            .HasDefaultValueSql("timezone('utc', now())");
    }
}