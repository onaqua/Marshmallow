using FastEndpoints;
using Marshmallow.Application.Queries;
using Marshmallow.Core.Entities;
using MediatR;

namespace Marshmallow.Endpoints;

public class GetTopicsEndpoint(IMediator mediator)
    : Endpoint<GetTopics.GetTopicsQuery, IReadOnlyCollection<Topic>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("api/topics");
    }

    public override Task HandleAsync(
        GetTopics.GetTopicsQuery query, 
        CancellationToken cancellationToken = default) =>
        mediator
            .Send<IReadOnlyCollection<Topic>>(query, cancellationToken);
}