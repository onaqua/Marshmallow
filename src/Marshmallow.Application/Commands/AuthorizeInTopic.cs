using ErrorOr;
using Marshmallow.Application.Authorization;
using Marshmallow.Core;
using Marshmallow.Core.Entities;
using Marshmallow.Infrastructure.Repositories;
using Marshmallow.Shared.Enums;
using MediatR;

namespace Marshmallow.Application.Commands;

public class AuthorizeInTopic
{ 
    public record Request(string TopicName, string GroupName, AuthorizationType AuthorizationType) : IRequest<ErrorOr<string>>;

    internal class Handler(
        IUnitOfWork unitOfWork,
        IGroupsRepository groupsRepository,
        ITopicsRepository topicsRepository,
        IConsumersRepository consumersRepository) : IRequestHandler<Request, ErrorOr<string>>
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

            var group = await groupsRepository
                .GetAsync(request.TopicName, request.GroupName, cancellationToken)
                .ConfigureAwait(false);

            if (group is null)
            {
                var createGroupResult = await Group
                    .Create(
                        topic: topic,
                        groupName: request.GroupName)
                    .ThenDoAsync(group => groupsRepository.AddAsync(group, cancellationToken));

                if (createGroupResult.IsError)
                {
                    return Error.Failure(
                        code: "Topic.Authorize",
                        description: "Cannot create group");
                }

                group = createGroupResult.Value;
            }

            var createConsumerResult = await Consumer
                .Create(group)
                .ThenDoAsync(consumer => consumersRepository.AddAsync(consumer, cancellationToken))
                .ThenDoAsync(consumer => unitOfWork.SaveChangesAsync(cancellationToken));

            if (createConsumerResult.IsError)
            {
                return Error.Failure(
                    code: "Topic.Authorize",
                    description: createConsumerResult.FirstError.Description);
            }

            //var authorizationToken = await authorizationService
            //    .AuthorizeConsumerAsync(createConsumerResult.Value, cancellationToken);

            //return authorizationToken;

            return default;
        }
    }
}