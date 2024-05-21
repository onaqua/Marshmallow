using ErrorOr;
using FastEndpoints;
using FluentValidation;
using Marshmallow.Application.Authorization;
using Marshmallow.Core;
using Marshmallow.Core.Entities;
using Marshmallow.Infrastructure.Repositories;
using Marshmallow.Shared.Enums;
using MediatR;

using Group = Marshmallow.Core.Entities.Group;

namespace Marshmallow.Application.Commands;

public class AuthorizeInTopic
{
    public record Request(string TopicName, string Name, AuthorizationType AuthorizationType) : IRequest<ErrorOr<string>>;

    internal class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.TopicName).NotEmpty();
        }
    }

    internal class Handler(
        IUnitOfWork unitOfWork,
        IGroupsRepository groupsRepository,
        ITopicsRepository topicsRepository,
        IConsumersRepository consumersRepository,
        IAuthorizationService authorizationService) : IRequestHandler<Request, ErrorOr<string>>
    {
        public async Task<ErrorOr<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            var topic = await topicsRepository
                .GetByNameAsync(request.TopicName, cancellationToken)
                .ConfigureAwait(false);

            if (topic is null)
            {
                return Error.NotFound(
                    code: "Topic.Subscribe.NotFound",
                    description: "Topic with current Id not found");
            }

            return request.AuthorizationType switch
            {
                AuthorizationType.Consumer => await AuthorizeConsumer(topic, request.Name, cancellationToken),
                AuthorizationType.Producer => await AuthorizeProducer(topic, request.Name, cancellationToken),
                _ => throw new InvalidOperationException()
            };
        }

        private async Task<ErrorOr<string>> AuthorizeConsumer(Topic topic, string groupName, CancellationToken cancellationToken = default)
        {
            var group = await groupsRepository
                .GetAsync(topic.Name, groupName, cancellationToken)
                .ConfigureAwait(false);

            if (group is null)
            {
                var createGroupResult = await Group
                    .Create(
                        topic: topic,
                        groupName: groupName)
                    .ThenDoAsync(group => groupsRepository.AddAsync(group, cancellationToken));

                if (createGroupResult.IsError)
                {
                    return Error.Failure(
                        code: "Topic.Authorize",
                        description: "Cannot create group");
                }

                group = createGroupResult.Value;
            }

            return await Consumer
                .Create(group)
                .ThenDoAsync(consumer => consumersRepository.AddAsync(consumer, cancellationToken))
                .ThenDoAsync(consumer => unitOfWork.SaveChangesAsync(cancellationToken))
                .ThenAsync(consumer => authorizationService.AuthorizeConsumerAsync(consumer, cancellationToken));
        }

        private Task<ErrorOr<string>> AuthorizeProducer(Topic topic, string producerName, CancellationToken cancellationToken = default)
        {
            return Producer
                .Create(topic, producerName)
                .ThenDo(topic.AddProducer)
                .ThenDoAsync(_ => unitOfWork.SaveChangesAsync(cancellationToken))
                .ThenAsync(producer => authorizationService.AuthorizeProducerAsync(producer, cancellationToken));
        }
    }
}