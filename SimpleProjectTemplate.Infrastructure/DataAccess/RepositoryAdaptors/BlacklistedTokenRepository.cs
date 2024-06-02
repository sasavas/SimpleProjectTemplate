using SimpleProjectTemplate.Domain.Features.Authentication;
using SimpleProjectTemplate.Infrastructure.DataAccess.Repositories;

namespace SimpleProjectTemplate.Infrastructure.DataAccess.RepositoryAdaptors;

internal class BlacklistedTokenRepository : SimpleRepositoryDecorator<BlacklistedToken, long>, IBlacklistedTokenRepository
{
    public BlacklistedTokenRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
    
    public bool IsTokenBlacklisted(string token)
    {
        var result = GetList().SingleOrDefault(bt => bt.Token == token);
        return result is not null;
    }
}