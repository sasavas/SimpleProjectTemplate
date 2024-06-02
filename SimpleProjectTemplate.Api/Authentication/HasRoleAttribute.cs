using SimpleProjectTemplate.SharedLibrary.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SimpleProjectTemplate.Api.Authentication;

public class HasRoleAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string[] _roles;
    
    public HasRoleAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Check if the user is authenticated
        if (context.HttpContext?.User.Identity is null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userRole = context.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == CustomClaims.Role)?.Value;
        
        // Check if the user has any of the specified roles
        bool authorized = !string.IsNullOrEmpty(userRole) && _roles.Contains(userRole);

        if (!authorized)
        {
            // User does not have the required roles
            context.Result = new ForbidResult();
        }
    }
}