using ErrorOr;
using FastEndpoints;
using Marshmallow.Application.Commands;
using MediatR;

namespace Marshmallow.Endpoints;

public class ConsumerAuthorization(IMediator mediator)
    : Endpoint<AuthorizeInTopic.Request>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("api/topics/authorize");
    }
    public override Task HandleAsync(
        AuthorizeInTopic.Request request, 
        CancellationToken cancellationToken) =>
        mediator
            .Send(request, cancellationToken)
            .ThenDoAsync(token => SendOkAsync(token));
}