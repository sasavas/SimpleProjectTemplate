using SimpleProjectTemplate.Domain.Abstract;

namespace SimpleProjectTemplate.Domain.Features.Authentication.ValueObjects;

public class PasswordResetValues : ValueObject
{
    public PasswordResetValues(string emailAddress, Guid code)
    {
        EmailAddress = emailAddress;
        Code = code;
        RequestedAt = DateTime.UtcNow;
    }

    public bool IsValid()
    {
        return (DateTime.UtcNow - RequestedAt).TotalMinutes <= 60
               && RequestConsumedAt == null;
    }

    public void SetRequestExpired()
    {
        RequestConsumedAt = DateTime.UtcNow;
    }

    public virtual User User { get; set; } = null!;
    public string EmailAddress { get; private set; }
    public Guid Code { get; private set; }
    private DateTime RequestedAt { get; set; }
    private DateTime? RequestConsumedAt { get; set; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return EmailAddress;
        yield return Code;
    }
}