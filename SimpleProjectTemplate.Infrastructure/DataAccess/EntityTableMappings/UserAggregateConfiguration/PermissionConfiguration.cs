using SimpleProjectTemplate.Domain.Features.Authentication.RoleModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleProjectTemplate.Infrastructure.DataAccess.EntityTableMappings.UserAggregateConfiguration;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder
            .ToTable("permissions")
            .HasKey(p => p.Id);
        builder.HasIndex(p => p.Name).IsUnique();
    }
}