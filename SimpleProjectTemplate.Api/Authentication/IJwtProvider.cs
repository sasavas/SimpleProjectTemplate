using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SimpleProjectTemplate.Application.UseCases.Users.Queries;
using SimpleProjectTemplate.SharedLibrary.Constants;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace SimpleProjectTemplate.Api.Authentication;

public interface IJwtProvider
{
    string Generate(Domain.Features.Authentication.User user);
    DateTime GetExpiry(string token);
    bool IsValid(string token);
}

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;
    private readonly ISender _mediatr;
    
    public JwtProvider(IOptions<JwtOptions> options, ISender mediatr)
    {
        _mediatr = mediatr;
        _options = options.Value;
    }

    public bool IsValid(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_options.SecretKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _options.Issuer,
                ValidateAudience = true,
                ValidAudience = _options.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // You can adjust the clock skew if necessary
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public DateTime GetExpiry(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
        var expiryDateUnix = jwtToken!.Claims.First(claim => claim.Type == "exp")!.Value;
        var expiryDateUnixSeconds = long.Parse(expiryDateUnix);
        var expiryDate = DateTimeOffset.FromUnixTimeSeconds(expiryDateUnixSeconds).DateTime;

        return expiryDate;
    }

    public string Generate(Domain.Features.Authentication.User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
        };

        // add roles
        var role = _mediatr.Send(new UserRoleQuery(user.Id)).Result;
        claims.Add(new Claim(CustomClaims.Role, role.Name));
        
        var signinCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddDays(7),
            signinCredentials);

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}