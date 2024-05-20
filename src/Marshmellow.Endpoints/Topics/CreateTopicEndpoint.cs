using ErrorOr;
using FastEndpoints;
using Marshmallow.Application.Commands;
using Marshmallow.Core.Entities;
using MediatR;

namespace Marshmallow.Endpoints;

public class CreateTopicEndpoint(IMediator mediator)
    : Endpoint<CreateTopic.Request, Topic>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("api/topics");
    }

    public override Task HandleAsync(
        CreateTopic.Request request,
        CancellationToken cancellationToken = default) =>
        mediator
            .Send(request, cancellationToken)
            .SwitchAsync(
                topic => SendAsync(topic, 200, cancellationToken),
                errors => this.SendFailureAsync(errors, 409, cancellationToken));
}
