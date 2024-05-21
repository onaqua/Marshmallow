using Grpc.Core;
using Marshmallow.Core.Entities;
using Marshmallow.Protos.Types;

namespace Marshmallow.Application.Hubs;

public interface ITopicsHub
{
    public IReadOnlyCollection<ITopicContext> Topics { get; }

    public Task SubscribeConsumerToTopicAsync(Topic topic, Consumer consumer, ServerCallContext context, IServerStreamWriter<MessageProto> serverStreamWriter, CancellationToken cancellationToken = default);

    public Task PublishIntoTopicAsync(Topic topicId, MessageProto messageProto, CancellationToken cancellationToken = default);
}
