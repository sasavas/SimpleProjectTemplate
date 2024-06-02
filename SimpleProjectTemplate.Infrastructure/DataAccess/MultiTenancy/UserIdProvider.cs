using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SimpleProjectTemplate.Domain.DataAccess;

namespace SimpleProjectTemplate.Infrastructure.DataAccess.MultiTenancy;

public class UserIdProvider(IHttpContextAccessor httpContextAccessor) : IUserIdProvider
{
    public Guid GetUserId()
    {
        var identity = httpContextAccessor?.HttpContext?.User.Identity;
        
        if (identity is null || !identity.IsAuthenticated) return Guid.Empty;
        
        var httpUserId
            = httpContextAccessor?
                .HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return httpUserId is not null ? Guid.Parse(httpUserId) : Guid.Empty;
    }
}