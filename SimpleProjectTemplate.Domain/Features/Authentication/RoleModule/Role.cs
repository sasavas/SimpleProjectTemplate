using SimpleProjectTemplate.Domain.Abstract;

namespace SimpleProjectTemplate.Domain.Features.Authentication.RoleModule;

public class Role : AggregateRoot<int>
{
    private Role(){}
    
    public Role(string name, IEnumerable<Permission> permissions)
    {
        Name = name;
        Permissions = permissions.ToArray();
    }

    public const string Admin = "Admin";
    public const string Member = "Member";
    public const string User = "User";
    public const string Guest = "Guest";

    public Role(int id, string name, IEnumerable<Permission> permissions)
        : this(name, permissions)
    {
        Id = id;
    }
    
    public string Name { get; set; }
    public ICollection<Permission> Permissions { get; set; }
}