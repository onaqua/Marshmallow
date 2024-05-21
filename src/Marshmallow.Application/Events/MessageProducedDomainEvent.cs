using Marshmallow.Core.Entities;
using MediatR;

namespace Marshmallow.Application.Events;

public record MessageProducedDomainEvent(Topic Topic, Message Message) : INotification;
