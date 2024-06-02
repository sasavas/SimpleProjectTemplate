using SimpleProjectTemplate.Domain.DataAccess.Contracts;

namespace SimpleProjectTemplate.Domain.Features.Authentication;

public interface IBlacklistedTokenRepository : IRepository<BlacklistedToken, long>
{
    bool IsTokenBlacklisted(string token);
}