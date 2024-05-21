using ErrorOr;
using FastEndpoints;
using Marshmallow.Application.Commands;
using MediatR;

namespace Marshmallow.Endpoints;

public class CreateTopicEndpoint(IMediator mediator)
    : Endpoint<CreateTopic.Request>
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
                topic => SendOkAsync(topic, cancellationToken),
                errors => SendAsync(errors, 409, cancellationToken));
}
