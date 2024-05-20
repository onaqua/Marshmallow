using Marshmallow.Core.Entities;
using Marshmallow.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Marshmallow.Infrastructure.Repositories;

public interface IReadConsumersRepository : IReadRepository<Guid, Consumer> { }
public interface IWriteConsumersRepository : IWriteRepository<Consumer> { }
public interface IConsumersRepository : IReadConsumersRepository, IWriteConsumersRepository { }

public class ConsumersRepository : RepositoryBase<Consumer>, IConsumersRepository
{
    public ConsumersRepository(DatabaseContext databaseContext) : base(databaseContext.Consumers)
    {
    }

    public async Task AddAsync(Consumer entity, CancellationToken cancellationToken = default) =>
        await Set.AddAsync(entity, cancellationToken).ConfigureAwait(false);

    public Task<Consumer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Set
            .Include(c => c.Group)
            .FirstAsync(cancellationToken);

    public void Remove(Consumer entity) => 
        Set.Remove(entity);

    public void Update(Consumer entity) =>
        Set.Update(entity);
}
