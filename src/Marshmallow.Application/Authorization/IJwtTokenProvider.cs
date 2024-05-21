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
    public string CreateProducerToken(Producer producer);
}

public class JwtTokenProvider : IJwtTokenProvider
{
    public string CreateConsumerToken(Consumer consumer)
    {
        var securityToken = new JwtSecurityToken(
            notBefore: DateTime.UtcNow,
            claims: CreateConsumerClaims(consumer),
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456test1524312345678901234")), SecurityAlgorithms.HmacSha256));

        var encodedToken = new JwtSecurityTokenHandler()
            .WriteToken(securityToken);

        return encodedToken;
    }

    public string CreateProducerToken(Producer producer)
    {
        var securityToken = new JwtSecurityToken(
            notBefore: DateTime.UtcNow,
            claims: CreateProducerClaims(producer),
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456test1524312345678901234")), SecurityAlgorithms.HmacSha256));

        var encodedToken = new JwtSecurityTokenHandler()
            .WriteToken(securityToken);

        return encodedToken;
    }

    private IReadOnlyCollection<Claim> CreateConsumerClaims(Consumer consumer)
    {
        var claims = new List<Claim>() {
            new Claim(InternalClaims.Role, InternalRoles.Consumer),
            new Claim(InternalClaims.ConsumerId, consumer.Id.ToString()),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, InternalRoles.Consumer)
        };

        return claims;
    }

    private IReadOnlyCollection<Claim> CreateProducerClaims(Producer producer)
    {
        var claims = new List<Claim>() {
            new Claim(InternalClaims.Role, InternalRoles.Producer),
            new Claim(InternalClaims.ProducerId, producer.Id.ToString()),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, InternalRoles.Producer)
        };

        return claims;
    }
}
