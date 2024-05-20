using Marshmallow.Core.Entities;
using Marshmallow.Shared.Enums;

namespace Marshmallow.Application.Authorization;

public interface IAuthorizationService
{
    public Task<string> AuthorizeConsumerAsync(Consumer consumer, CancellationToken cancellationToken = default);
}
