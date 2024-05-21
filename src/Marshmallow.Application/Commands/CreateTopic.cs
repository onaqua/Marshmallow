using ErrorOr;
using FastEndpoints;
using FluentValidation;
using Marshmallow.Core;
using Marshmallow.Core.Entities;
using Marshmallow.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Marshmallow.Application.Commands;

public class CreateTopic
{
    public record Request(string Name) : IRequest<ErrorOr<Topic>>;

    internal class Validator : Validator<Request>
    {
        public Validator(IServiceProvider provider) 
        {
            var repository = provider
                .CreateAsyncScope()
                .Resolve<ITopicsRepository>();

            RuleFor(request => request.Name)
                .MustAsync(repository.IsUniqueByNameAsync)
                .WithErrorCode("Topic.Create.Validation")
                .WithMessage("Name of topic must be unique.");
        }
    }

    internal class Handler(ITopicsRepository repository, IUnitOfWork unitOfWork) 
        : IRequestHandler<Request, ErrorOr<Topic>>
    {
        public Task<ErrorOr<Topic>> Handle(Request request, CancellationToken cancellationToken)
        {
            return Topic
               .Create(name: request.Name)
               .ThenDoAsync(topic => repository.AddAsync(topic, cancellationToken))
               .ThenDoAsync(topic => unitOfWork.SaveChangesAsync(cancellationToken));
        }
    }
}