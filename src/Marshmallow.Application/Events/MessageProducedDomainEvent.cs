using Marshmallow.Core.Entities;
using Marshmallow.Protos.Types;
using MediatR;

namespace Marshmallow.Application.Events;

public record MessageProducedDomainEvent(Topic Topic, PayloadProto Payload) : INotification;
