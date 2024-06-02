using SimpleProjectTemplate.Application.UseCases.Users.Commands;
using SimpleProjectTemplate.Domain.Features.Authentication.ValueObjects;

namespace SimpleProjectTemplate.Integration.Test.Tests;

public class SampleTest : BaseIntegrationTest
{
    public SampleTest(TestWebApplicationFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task OnBoarding()
    {
        //TODO make this test run
        var user = await Sender.Send(new RegisterUserCommand("test3@test.com", "test", "en"));
        
        Factory.OverrideUserIdProviderValue(user.Id);
        
        await Sender.Send(new CompleteOnboardingCommand("Ali", "Atmaca", "male", DateOnly.MaxValue));
        
        var onboardedUser = AppDbContext.Users.Single(u => u.Email == new Email("test3@test.com"));
        Assert.Equal("male", onboardedUser.Gender!.Value);
        Assert.Equal("Atmaca", onboardedUser.LastName);
    }

    [Fact]
    public async Task UserPasswordResetTest()
    {
        const string email = "test2@test.com";

        await Sender.Send(
            new RegisterUserCommand(
                email,
                "test",
                "en"
            ));

        await Sender.Send(new ResetPasswordCommand(email));

        var resetCode = AppDbContext.Users
            .First(user => user.Email == new Email(email))
            .PasswordResetValues.First()
            .Code;

        await Sender.Send(new VerifyPasswordResetCommand(resetCode, "newpass"));

        Assert.Equal("newpass", AppDbContext.Users
            .First(user => user.Email == new Email(email))
            .Password.Value);
    }
}