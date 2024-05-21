using Grpc.Core;
using Marshmallow.Core.Entities;
using Marshmallow.Protos.Types;

namespace Marshmallow.Application.Hubs;

public class TopicContext(Topic topic) : ITopicContext
{
    private readonly HashSet<IConsumerContext> _connectedConsumers = new HashSet<IConsumerContext>();
    public Topic Topic => topic;

    public IReadOnlyCollection<IConsumerContext> ConnectedConsumers => _connectedConsumers;

    public async Task ConnectConsumerAsync(
        Consumer consumer,
        ServerCallContext serverCallContext,
        IServerStreamWriter<MessageProto> serverStreamWriter, CancellationToken cancellationToken = default)
    {
        var consumerContext = new ConsumerContext(consumer, serverCallContext, serverStreamWriter);

        _connectedConsumers.Add(consumerContext);

        await Task
            .Delay(Timeout.Infinite, cancellationToken)
            .ContinueWith(connection =>
            {
                if (cancellationToken.IsCancellationRequested || connection.IsCanceled || connection.IsFaulted || connection.IsCompleted || connection.IsCompletedSuccessfully)
                {
                    _connectedConsumers.Remove(consumerContext);
                }
            });
    }

    public Task PublishAsync(
        MessageProto messageProto, CancellationToken cancellationToken = default)
    {
        var consumer = _connectedConsumers.FirstOrDefault();

        if (consumer is null)
        {
            return Task.CompletedTask;
        }

        return consumer.PublishAsync(messageProto, cancellationToken);
    }
}
