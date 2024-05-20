using ErrorOr;
using Marshmallow.Core;
using Marshmallow.Core.Entities;
using Marshmallow.Infrastructure.Repositories;
using MediatR;

namespace Marshmallow.Application.Commands;

public class CreateTopic
{
    public record Request(string Name) : IRequest<ErrorOr<Topic>>;

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