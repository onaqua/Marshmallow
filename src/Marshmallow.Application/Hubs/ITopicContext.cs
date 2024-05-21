using Grpc.Core;
using Marshmallow.Core.Entities;
using Marshmallow.Protos.Types;

namespace Marshmallow.Application.Hubs;

public interface ITopicContext
{
    public Topic Topic { get; }

    public IReadOnlyCollection<IConsumerContext> ConnectedConsumers { get; }

    public Task PublishAsync(MessageProto messageProto, CancellationToken cancellationToken = default);

    public Task ConnectConsumerAsync(Consumer consumer, ServerCallContext serverCallContext, IServerStreamWriter<MessageProto> serverStreamWriter, CancellationToken cancellationToken = default);
}
