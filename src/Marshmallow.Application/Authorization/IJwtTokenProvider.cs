using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Marshmallow.Core.Entities;
using Marshmallow.Shared.Constants;
using Microsoft.IdentityModel.Tokens;

namespace Marshmallow.Application.Authorization;

public interface IJwtTokenProvider : IJwtConsumerTokenProvider, IJwtProducerTokenProvider { }


public interface IJwtConsumerTokenProvider
{
    public string CreateConsumerToken(Consumer consumer);
}

public interface IJwtProducerTokenProvider
{

}

public class JwtTokenProvider : IJwtTokenProvider
{
    public string CreateConsumerToken(Consumer consumer)
    {
        var securityToken = new JwtSecurityToken(
            notBefore: DateTime.UtcNow,
            claims: CreateClaims(consumer),
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456test1524312345678901234")), SecurityAlgorithms.HmacSha256));

        var encodedToken = new JwtSecurityTokenHandler()
            .WriteToken(securityToken);

        return encodedToken;
    }

    private IReadOnlyCollection<Claim> CreateClaims(Consumer consumer)
    {
        var claims = new List<Claim>() {
            new Claim(InternalClaims.Role, InternalRoles.Consumer),
            new Claim(InternalClaims.ConsumerId, consumer.Id.ToString()),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, InternalRoles.Consumer)
        };

        return claims;
    }
}
