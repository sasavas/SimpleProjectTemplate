using SimpleProjectTemplate.Domain.Features.Authentication.RoleModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleProjectTemplate.Infrastructure.DataAccess.EntityTableMappings.UserAggregateConfiguration;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder
            .ToTable("role_permissions")
            .HasKey(x => new { x.RoleId, x.PermissionId });
    }
}