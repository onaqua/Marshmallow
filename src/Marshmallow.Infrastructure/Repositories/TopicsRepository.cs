using Marshmallow.Core.Entities;
using Marshmallow.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Marshmallow.Infrastructure.Repositories;

public interface IReadTopicsRepository : IReadRepository<Guid, Topic>
{
    public Task<IReadOnlyCollection<Topic>> SearchByNameAsync(
        string? name = "", 
        int offset = 0, 
        int quantity = 5, 
        CancellationToken cancellationToken = default);
    
    public Task<bool> IsUniqueByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    public Task<Topic?> GetByNameAsync(
        string name, 
        CancellationToken cancellationToken = default);
}

public interface IWriteTopicsRepository : IWriteRepository<Topic>
{
}

public interface ITopicsRepository : IWriteTopicsRepository, IReadTopicsRepository
{
}

internal class TopicsRepository(DatabaseContext databaseContext) : 
    RepositoryBase<Topic>(databaseContext.Topics), ITopicsRepository
{
    public async Task AddAsync(Topic entity, CancellationToken cancellationToken = default) =>
        await Set.AddAsync(entity, cancellationToken);

    public Task<Topic?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Set.SingleOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);

    public Task<Topic?> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        Set
            .Include(x => x.Groups)!
            .ThenInclude(x => x.Consumers)
            .Include(x => x.Producers)
            .SingleOrDefaultAsync(e => e.Name.Equals(name), cancellationToken);

    public async Task<bool> IsUniqueByNameAsync(string name, CancellationToken cancellationToken = default) =>
        !await Set.AnyAsync(e => e.Name.Equals(name), cancellationToken);

    public void Remove(Topic entity) =>
        Set.Remove(entity);

    public async Task<IReadOnlyCollection<Topic>> SearchByNameAsync(
        string name = "", 
        int offset = 0, 
        int quantity = 5, 
        CancellationToken cancellationToken = default) =>
        await Set
            .Where(e => EF.Functions.Like(e.Name, $"%{name}%"))
            .OrderByDescending(e => e.Name)
            .Skip(offset)
            .Take(quantity)
            .ToListAsync(cancellationToken);

    public void Update(Topic entity) =>
        Set.Update(entity);
}
