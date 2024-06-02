using SimpleProjectTemplate.Domain.Features.Authentication;
using SimpleProjectTemplate.Domain.Features.Authentication.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleProjectTemplate.Infrastructure.DataAccess.EntityTableMappings.UserAggregateConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUser(builder);
    }

    private static void ConfigureUser(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users").HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        builder.Property(user => user.UserName).HasColumnName("username");

        builder.Property(u => u.Email).IsRequired()
            .HasConversion<string>(
                e => e.Value,
                s => new Email(s));

        builder.Property(u => u.Password)
            .HasConversion<string>(
                e => e.Value,
                s => new Password(s))
            .HasColumnName("password");

        builder.Property(u => u.Gender)
            .HasConversion<string>(
                gender => gender.Value,
                s => new Gender(s))
            .HasColumnName("gender");

        builder
            .HasOne(u => u.Role)
            .WithMany().HasForeignKey("role_id");

        builder.OwnsMany(user => user.PasswordResetValues)
            .WithOwner(pass => pass.User);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.UserName).IsUnique();
    }
}