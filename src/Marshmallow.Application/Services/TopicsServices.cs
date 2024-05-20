using Grpc.Core;
using Marshmallow.Core;
using Marshmallow.Core.Entities;
using Marshmallow.Infrastructure.Repositories;
using Marshmallow.Protos.Services;
using Marshmallow.Protos.Types;

using static Marshmallow.Protos.Services.TopicService;

namespace Marshmallow.Application.Services;

public class TopicService(
    IUnitOfWork unitOfWork,
    IStreamClients streamClients,
    IConsumersRepository consumersRepository) : TopicServiceBase
{
    public override async Task Subscribe(
        SubscribeProtoRequest request,
        IServerStreamWriter<MessageProto> responseStream,
        ServerCallContext context)
    {
        streamClients.Clients.Add(responseStream);

        await Task.Delay(Timeout.Infinite, context.CancellationToken);
    }
}

public interface ITopicsHub
{
    public IReadOnlyCollection<ITopicContext> Topics { get; }

    public Task ConnectConsumerAsync(Consumer consumer, IServerStreamWriter<MessageProto> serverStreamWriter, CancellationToken cancellationToken = default);

    public Task PublishIntoTopicAsync(Guid topicId, MessageProto messageProto, CancellationToken cancellationToken = default);
}

public class TopicsHub : ITopicsHub
{
    public IReadOnlyCollection<ITopicContext> Topics => throw new NotImplementedException();

    public Task ConnectConsumerAsync(Consumer consumer, IServerStreamWriter<MessageProto> serverStreamWriter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task PublishIntoTopicAsync(Guid topicId, MessageProto messageProto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

public interface ITopicContext
{
    public Topic Topic { get; }

    public IReadOnlyCollection<IConsumerContext> ConnectedConsumers { get; }

    public Task PublishAsync(MessageProto messageProto, CancellationToken cancellationToken = default);

    public Task ConnectConsumerAsync(Consumer consumer, ServerCallContext serverCallContext, IServerStreamWriter<MessageProto> serverStreamWriter, CancellationToken cancellationToken = default);
}

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

public class ConsumerContext(
    Consumer consumer,
    ServerCallContext serverCallContext,
    IServerStreamWriter<MessageProto> serverStreamWriter) : IConsumerContext
{
    private readonly ServerCallContext _serverCallContext = serverCallContext;
    private readonly IServerStreamWriter<MessageProto> _serverStreamWriter = serverStreamWriter;

    public Consumer Consumer => consumer;

    public async Task PublishAsync(MessageProto messageProto, CancellationToken cancellationToken = default)
    {
        await _serverStreamWriter
            .WriteAsync(messageProto, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}


public interface IConsumerContext
{
    public Consumer Consumer { get; }

    public Task PublishAsync(MessageProto messageProto, CancellationToken cancellationToken = default);
}