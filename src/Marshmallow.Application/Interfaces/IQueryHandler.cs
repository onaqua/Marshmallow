using FastEndpoints;
using MediatR;

namespace Marshmallow.Application.Interfaces;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}
