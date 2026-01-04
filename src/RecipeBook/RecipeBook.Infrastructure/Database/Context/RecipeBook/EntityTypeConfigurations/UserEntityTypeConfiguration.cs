using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeBook.Domain.Entities;

namespace RecipeBook.Infrastructure.Database.Context.RecipeBook.EntityTypeConfigurations;

public class UserEntityTypeConfiguration : BaseEntityTypeConfiguration<UserEntity>
{
    protected override string  TableName => "user";
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.Property(u => u.Username)
            .IsRequired()
            .IsUnicode()
            .HasColumnName("user_name")
            .HasMaxLength(128);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasColumnName("email")
            .HasMaxLength(312);
        
        builder.Property(u => u.Password)
            .IsRequired()
            .HasColumnName("password")
            .HasMaxLength(512);

        builder.Property(x => x.DateOfBirth)
            .IsRequired(false)
            .HasColumnName("date_of_birth");

        // builder.Property(x => x.IsEmailConfirmed)
        //     .HasColumnName("is_email_confirmed");
        //
        // builder.Property(x => x.EmailConfirmationToken)
        //     .HasColumnName("email_confirmation_token")
        //     .IsRequired(false);

        builder
            .HasMany(x => x.Recipes)
            .WithOne(x => x.User)
            .HasConstraintName("fk_user_recipe");
        
        base.Configure(builder);
    }
}