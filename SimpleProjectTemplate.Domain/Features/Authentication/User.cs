using SimpleProjectTemplate.Domain.Abstract;
using SimpleProjectTemplate.Domain.Features.Authentication.RoleModule;
using SimpleProjectTemplate.Domain.Features.Authentication.ValueObjects;

namespace SimpleProjectTemplate.Domain.Features.Authentication;

public class User : AggregateRoot<Guid>
{
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Email Email { get; set; }
    public Password Password { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public Role Role { get; set; }
    public bool IsVerified { get; set; }
    public Guid VerificationCode { get; set; }
    public ICollection<PasswordResetValues> PasswordResetValues { get; private set; } = new List<PasswordResetValues>();

    public User()
    {
    }

    public static User Create(
        Email email, Password password, Role role, Guid verificationCode,
        string? userName = null, string? firstName = null, string? lastName = null, DateOnly? dateOfBirth = null,
        Gender? gender = null)
    {
        var user = new User
        {
            UserName = userName,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
            DateOfBirth = dateOfBirth,
            Gender = gender,
            Role = role, //TODO how to create role???
            VerificationCode = verificationCode
        };

        //TODO raise domain event (UserCreatedEvent)

        return user;
    }

    public bool HasValidPasswordResetRequest()
    {
        return PasswordResetValues.Any(resetRequest => resetRequest.IsValid());
    }

    public void UpdatePassword(string newPassword)
    {
        Password = new Password(newPassword);
    }
}