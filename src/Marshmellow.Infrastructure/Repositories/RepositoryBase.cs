using Marshmallow.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marshmallow.Infrastructure.Repositories;

public abstract class RepositoryBase<T> where T : EntityBase
{ 
    public RepositoryBase(DbSet<T> set)
    {
        Set = set;
    }

    public DbSet<T> Set { get; init; }
}
