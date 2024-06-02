using SimpleProjectTemplate.Domain.Abstract;

namespace SimpleProjectTemplate.Domain.Features.Authentication.RoleModule;

public class Permission : Entity<int>
{
    public string Name { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; }
    public ICollection<Role> Roles { get; set; }
    
    public const string ReadPermission = "Read";
    public const string WritePermission = "Write";
    
    private Permission(){}
    
    public Permission(string name)
    {
        Name = name;
    }

    public Permission(int id, string name) : this(name)
    {
        Id = id;
    }
}