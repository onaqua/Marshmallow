using Marshmallow.Core.Entities;

namespace Marshmallow.Application.Authorization;

public interface IJwtTokenProvider
{
    public string CreateConsumerToken(Consumer consumer);
}

