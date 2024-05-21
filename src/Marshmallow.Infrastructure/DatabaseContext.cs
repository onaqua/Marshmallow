using Marshmallow.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Marshmallow.Infrastructure;

public class PublishDomainEventsInterceptor(IPublisher publisher) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await PublishDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEvents(DbContext? dbContext, CancellationToken cancellationToken = default)
    {
        if (dbContext is null)
        {
            return;
        }

        var entitiesWithDomainEvents = dbContext.ChangeTracker.Entries<IHasDomainEvents>()
            .Where(entry => entry.Entity.DomainEvents.Count > 0)
            .Select(entry => entry.Entity)
            .ToList();

        var domainEvents = entitiesWithDomainEvents
            .SelectMany(entry => entry.DomainEvents)
            .ToList();

        entitiesWithDomainEvents.ForEach(entity => entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }
    }
}

public class DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions, PublishDomainEventsInterceptor publishDomainEventsInterceptor) : DbContext(dbContextOptions)
{
    

    public DbSet<Topic> Topics { get; init; }
    public DbSet<Header> Headers { get; init; }
    public DbSet<Message> Messages { get; init; }
    public DbSet<Consumer> Consumers { get; init; }
    public DbSet<Group> Groups { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(publishDomainEventsInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>(entity =>
        {
            entity.OwnsOne(m => m.Payload);
        });

        modelBuilder
            .Ignore<IReadOnlyCollection<INotification>>()
            .ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }
}