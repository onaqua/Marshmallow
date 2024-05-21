using Grpc.Core;
using Marshmallow.Core.Entities;
using Marshmallow.Protos.Types;

namespace Marshmallow.Application.Hubs;

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
