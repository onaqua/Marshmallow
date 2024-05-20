using Marshmallow.Core;

namespace Marshmallow.Infrastructure;

public class UnitOfWork(DatabaseContext databaseContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return databaseContext.SaveChangesAsync(cancellationToken);
    }
}