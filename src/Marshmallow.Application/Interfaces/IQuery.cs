using FastEndpoints;
using MediatR;

namespace Marshmallow.Application.Interfaces;

public interface IQuery : IRequest
{
}

public interface IQuery<T> : IRequest<T>, IQuery
{ 
}
