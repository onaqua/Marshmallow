using ErrorOr;
using Marshmallow.Application.Interfaces;
using Marshmallow.Core.Entities;
using Marshmallow.Extensions.Extensions;
using Marshmallow.Infrastructure.Repositories;

namespace Marshmallow.Application.Queries;

public class GetTopic
{
    public record GetTopicByIdQuery(Guid Id) : IQuery<ErrorOr<Topic>>;

    internal class Handler(ITopicsRepository topicsRepository) : IQueryHandler<GetTopicByIdQuery, ErrorOr<Topic>>
    {
        public async Task<ErrorOr<Topic>> Handle(GetTopicByIdQuery request, CancellationToken cancellationToken)
        {
            var topic = await topicsRepository
                 .GetByIdAsync(request.Id, cancellationToken);

            if (topic.IsNull())
            {
                return Error.NotFound(
                    code: "Endpoints.Topic.Get",
                    description: "Topic with current Id not found");
            }

            return topic;
        }
    }
}