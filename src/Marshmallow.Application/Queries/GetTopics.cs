using Marshmallow.Application.Interfaces;
using Marshmallow.Core.Entities;
using Marshmallow.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Marshmallow.Application.Queries;

public class GetTopics 
{
    public record GetTopicsQuery(
        [FromQuery] string? SearchQuery = "",
        int Offset = 1,
        int Quantity = 5) : IQuery<IReadOnlyCollection<Topic>>;

    internal class Handler(ITopicsRepository topicsRepository) : IQueryHandler<GetTopicsQuery, IReadOnlyCollection<Topic>>
    {
        public Task<IReadOnlyCollection<Topic>> Handle(GetTopicsQuery request, CancellationToken cancellationToken) =>
            topicsRepository.SearchByNameAsync(
                name: request.SearchQuery,
                offset: request.Offset,
                quantity: request.Quantity,
                cancellationToken: cancellationToken);
    }
}