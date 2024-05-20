using Marshmallow.Core.Entities;

namespace Marshmallow.Core.Repositories;

public interface IReadRepository<TKey, TEntity>
    where TKey : IEquatable<TKey>
    where TEntity : EntityBase
{
    public Task<TEntity?> GetByIdAsync(
        TKey id, 
        CancellationToken cancellationToken = default);
}

public interface IWriteRepository<TEntity>
    where TEntity : EntityBase
{
    public Task AddAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default);

    public void Update(TEntity entity);

    public void Remove(TEntity entity);
}

