using Google.Protobuf;
using Marshmallow.Application.Events;
using Marshmallow.Application.Hubs;
using Marshmallow.Protos.Types;
using MediatR;

namespace Marshmallow.Application.EventsHandlers;

public class MessageProducedDomainEventHandler(ITopicsHub topicsHub) : INotificationHandler<MessageProducedDomainEvent>
{
    public Task Handle(MessageProducedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var messageProto = new MessageProto
        {
            Payload = new PayloadProto
            {
                Value = ByteString.CopyFrom(domainEvent.Message.Payload.Value)
            }
        };

        topicsHub.PublishIntoTopicAsync(domainEvent.Topic, messageProto);

        return Task.CompletedTask;
    }
}
