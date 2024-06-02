using SimpleProjectTemplate.Domain.Features.Authentication.RoleModule;
using SimpleProjectTemplate.Infrastructure.DataAccess;

namespace SimpleProjectTemplate.Api.Utils;

public static class DatabaseSeeder
{
    public static void SeedDatabase(AppDbContext context)
    {
        // look up tables / reference data
        SeedRolesAndPermissions(context);
        
        context.SaveChanges();
    }

    private static void SeedRolesAndPermissions(AppDbContext context)
    {
        if (context.Roles.Any()) return;
        
        var readPermission = new Permission(1, Permission.ReadPermission);
        var writePermission = new Permission(2, Permission.WritePermission);
        
        var admin = new Role(1, Role.Admin, new[] { readPermission, writePermission });
        var member = new Role(2, Role.Member, new[] { readPermission, writePermission });
        var user = new Role(3, Role.User, new[] { readPermission, writePermission });
        var guest = new Role(4, Role.Guest, new[] { readPermission, writePermission });

        context.Roles.AddRange(admin, member, user, guest);
        context.SaveChanges();
    }
}