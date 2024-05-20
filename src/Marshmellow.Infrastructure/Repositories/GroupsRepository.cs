using Marshmallow.Core.Entities;
using Marshmallow.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Marshmallow.Infrastructure.Repositories;

public interface IReadGroupsRepository : IReadRepository<Guid, Group>
{
    public Task<Group?> GetAsync(
        string topicName, 
        string groupName, 
        CancellationToken cancellationToken = default);
}

public interface IWriteGroupsRepository : IWriteRepository<Group> { }
public interface IGroupsRepository : IWriteGroupsRepository, IReadGroupsRepository { }

internal class GroupsRepository : RepositoryBase<Group>, IGroupsRepository
{
    public GroupsRepository(DatabaseContext databaseContext) : base(databaseContext.Groups)
    {
    }

    public async Task AddAsync(Group entity, CancellationToken cancellationToken = default) =>
        await Set.AddAsync(entity, cancellationToken);

    public async Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await Set.SingleOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);

    public async Task<Group?> GetAsync(string topicName, string groupName, CancellationToken cancellationToken = default) =>
        await Set
            .Include(group => group.Topic)
            .SingleOrDefaultAsync(
                group => 
                    group.Name.Equals(groupName) &&
                    group.Topic.Name.Equals(topicName),
                cancellationToken);

    public void Remove(Group entity)
    {
        throw new NotImplementedException();
    }

    public void Update(Group entity)
    {
        throw new NotImplementedException();
    }
}
