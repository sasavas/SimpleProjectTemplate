using SimpleProjectTemplate.Domain.Features.Authentication.RoleModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleProjectTemplate.Infrastructure.DataAccess.EntityTableMappings.UserAggregateConfiguration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles").HasKey(r => r.Id);
        
        builder.HasIndex(r => r.Name).IsUnique();

        builder.HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity<RolePermission>();
    }
}