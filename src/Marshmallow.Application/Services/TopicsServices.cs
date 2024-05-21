using Grpc.Core;
using Marshmallow.Application.Exceptions;
using Marshmallow.Application.Hubs;
using Marshmallow.Extensions.Extensions;
using Marshmallow.Infrastructure.Repositories;
using Marshmallow.Protos.Services;
using Marshmallow.Protos.Types;
using Marshmallow.Shared.Constants;
using Microsoft.AspNetCore.Authorization;

using static Marshmallow.Protos.Services.TopicService;

namespace Marshmallow.Application.Services;

public class TopicService(
    ITopicsHub topicsHub,
    ITopicsRepository topicsRepository,
    IConsumersRepository consumersRepository) : TopicServiceBase
{
    [Authorize(Roles = InternalRoles.Consumer)]
    public override async Task Subscribe(
        SubscribeProtoRequest request,
        IServerStreamWriter<MessageProto> responseStream,
        ServerCallContext context)
    {
        var consumerId = context
            .GetConsumerId();

        var consumer = await consumersRepository
            .GetByIdAsync(consumerId, context.CancellationToken);

        if (consumer is null)
        {
            throw Errors.Consumer.NotFound(consumerId);
        }

        var topic = await topicsRepository
            .GetByNameAsync(request.TopicName, context.CancellationToken);

        if (topic is null)
        {
            throw Errors.Topic.NotFound(request.TopicName);
        }

        await topicsHub
            .SubscribeConsumerToTopicAsync(topic, consumer, context, responseStream)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}