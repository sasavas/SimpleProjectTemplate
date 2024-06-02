using SimpleProjectTemplate.Domain.Abstract;

namespace SimpleProjectTemplate.Domain.Features.Authentication;

public class BlacklistedToken : AggregateRoot<long>
{
    public string Token { get; set; }
    public DateTime BlacklistedAt { get; set; }
    public DateTime TokenExpiryDate { get; set; }
    
    private BlacklistedToken(){}
    
    private BlacklistedToken(string token, DateTime blacklistedAt, DateTime tokenExpiryDate)
    {
        Token = token;
        BlacklistedAt = blacklistedAt;
        TokenExpiryDate = tokenExpiryDate;
    }

    public static BlacklistedToken Create(string token, DateTime blacklistedAt, DateTime tokenExpiryDate)
    {
        return new BlacklistedToken(token, blacklistedAt, tokenExpiryDate);
    }
}