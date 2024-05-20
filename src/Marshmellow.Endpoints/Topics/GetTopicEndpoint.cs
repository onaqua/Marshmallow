using ErrorOr;
using FastEndpoints;
using Marshmallow.Application.Queries;
using Marshmallow.Core.Entities;
using MediatR;

namespace Marshmallow.Endpoints;

public class GetTopicEndpoint(IMediator mediator)
    : Endpoint<GetTopic.GetTopicByIdQuery, Topic>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("api/topics/{Id}");
    }

    public override Task HandleAsync(
        GetTopic.GetTopicByIdQuery query,
        CancellationToken cancellationToken) =>
        mediator
            .Send<ErrorOr<Topic>>(query, cancellationToken)
            .SwitchAsync(
                value => SendOkAsync(value, cancellationToken),
                errors => SendNotFoundAsync(cancellationToken));
}