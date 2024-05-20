using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Marshmallow.Infrastructure.Jobs;

internal class MigrationsJob(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        using var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        await database.Database.MigrateAsync(stoppingToken);
    }
}
