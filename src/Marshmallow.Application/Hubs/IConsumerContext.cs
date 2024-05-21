using Marshmallow.Core.Entities;
using Marshmallow.Protos.Types;

namespace Marshmallow.Application.Hubs;

public interface IConsumerContext
{
    public Consumer Consumer { get; }

    public Task PublishAsync(MessageProto messageProto, CancellationToken cancellationToken = default);
}