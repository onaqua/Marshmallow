using Grpc.Core;
using Marshmallow.Core.Entities;
using Marshmallow.Protos.Types;

namespace Marshmallow.Application.Hubs;

public class TopicsHub : ITopicsHub
{
    private readonly HashSet<ITopicContext> _topics = new HashSet<ITopicContext>();
    public IReadOnlyCollection<ITopicContext> Topics => _topics;

    public Task SubscribeConsumerToTopicAsync(
        Topic topic,
        Consumer consumer,
        ServerCallContext context,
        IServerStreamWriter<MessageProto> serverStreamWriter, CancellationToken cancellationToken = default)
    {
        var topicContext = Topics
            .SingleOrDefault(context => context.Topic.Id.Equals(topic.Id));

        if (topicContext is null)
        {
            topicContext = new TopicContext(topic);

            _topics.Add(topicContext);
        }

        return topicContext
            .ConnectConsumerAsync(consumer, context, serverStreamWriter, cancellationToken);
    }

    public Task PublishIntoTopicAsync(Topic topic, MessageProto messageProto, CancellationToken cancellationToken = default)
    {
        var topicContext = Topics
            .SingleOrDefault(context => context.Topic.Id.Equals(topic.Id));

        if (topicContext is null)
        {
            topicContext = new TopicContext(topic);

            _topics.Add(topicContext);
        }

        return topicContext
            .PublishAsync(messageProto, cancellationToken);
    }
}
