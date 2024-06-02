using SimpleProjectTemplate.Api.Authentication;
using SimpleProjectTemplate.Api.DTOs.User;
using SimpleProjectTemplate.Application.UseCases.Users.Commands;
using SimpleProjectTemplate.Application.UseCases.Users.Queries;
using SimpleProjectTemplate.Domain.Features.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleProjectTemplate.Domain.Features.Authentication.DataAccess;

namespace SimpleProjectTemplate.Api.Controllers;

public class UsersController(
    ISender _sender,
    IJwtProvider _jwtProvider,
    IUserRepository _userRepository) : BaseController
{
    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterUserCommand command, CancellationToken token)
    {
        await _sender.Send(command, token);
        return NoContent();
    }

    [Authorize]
    [HttpPost("delete")]
    public async Task<ActionResult> Delete([FromBody] DeleteUserCommand command, CancellationToken token)
    {
        await _sender.Send(command, token);
        return NoContent();
    }


    [HttpPost("verify")]
    public async Task<ActionResult> VerifyUser([FromBody] VerifyUserCommand command, CancellationToken token)
    {
        var result = await _sender.Send(command, token);
        return Ok(result);
    }
    
    [Authorize]
    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId, CancellationToken token)
    {
        await _sender.Send(new DeleteUserCommand(userId), token);
        return NoContent();
    }

    [HttpGet("registerInfo")]
    public async Task<IActionResult> RegisterUserInfo()
    {
        var result = await _sender.Send(new RegisterUserInfoQuery());
        return Ok(result);
    }

    [HttpGet("resetPassword/{emailAddress}")]
    public async Task<IActionResult> ResetPassword(string emailAddress)
    {
        await _sender.Send(new Application.UseCases.Users.Commands.ResetPasswordCommand(emailAddress));
        return NoContent();
    }


    [HttpPost("verifyPasswordReset")]
    public async Task<IActionResult> VerifyPasswordReset(VerifyPasswordResetRequestDTO requestDto)
    {
        await _sender.Send(new Application.UseCases.Users.Commands.VerifyPasswordResetCommand(requestDto.code, requestDto.newPassword));
        return Ok(true);
    }

    [HttpPost("CompleteOnboarding")]
    public async Task<ActionResult> CompleteOnboarding(OnboardingRequestDTO request)
    {
        await _sender.Send(
            new CompleteOnboardingCommand(
                request.FirstName, request.LastName, request.Gender, request.DateOfBirth));
        
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginQuery query, CancellationToken cancellationToken)
    {
        User user = await _sender.Send(query, cancellationToken);

        if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var authHeaderVal = authHeader.ToString();
            if (authHeaderVal.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeaderVal["Bearer ".Length..].Trim();
                if (_jwtProvider.IsValid(token))
                {
                    return Ok(new LoginResponseDTO(user.Id, token, _jwtProvider.GetExpiry(token)));
                }
            }
        }

        string jwtToken = _jwtProvider.Generate(user);
        return Ok(new LoginResponseDTO(user.Id, jwtToken, _jwtProvider.GetExpiry(jwtToken)));
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task<ActionResult> Logout()
    {
        var authHeader = HttpContext.Request.Headers.Authorization.First();
        if (string.IsNullOrEmpty(authHeader))
        {
            return BadRequest("No auth header was found. User may not have been logged in already.");
        }
        
        var token = authHeader["Bearer ".Length..].Trim();
        await _sender.Send(new LogoutUserCommand(token, _jwtProvider.GetExpiry(token)));
        return Ok();
    }
}