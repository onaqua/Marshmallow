using ErrorOr;
using FastEndpoints;
using Marshmallow.Application.Commands;
using MediatR;

namespace Marshmallow.Endpoints;

public class TopicAuthorizationEndpoint(IMediator mediator)
    : Endpoint<AuthorizeInTopic.Request>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("api/topics/authorize");
    }

    public override Task HandleAsync(
        AuthorizeInTopic.Request request, 
        CancellationToken cancellationToken = default) =>
        mediator
            .Send(request, cancellationToken)
            .SwitchAsync(
                token => SendOkAsync(token, cancellationToken),
                errors => SendAsync(errors, 409, cancellationToken));
}