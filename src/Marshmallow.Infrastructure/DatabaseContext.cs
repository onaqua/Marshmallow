using Marshmallow.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marshmallow.Infrastructure;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<Topic> Topics { get; init; }
    public DbSet<Header> Headers { get; init; }
    public DbSet<Message> Messages { get; init; }
    public DbSet<Consumer> Consumers { get; init; }
    public DbSet<Group> Groups { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>(entity =>
        {
            entity.OwnsOne(m => m.Payload);
        });
    }
}