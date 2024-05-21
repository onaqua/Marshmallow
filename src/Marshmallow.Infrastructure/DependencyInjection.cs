using Marshmallow.Core;
using Marshmallow.Infrastructure.Configurations;
using Marshmallow.Infrastructure.Jobs;
using Marshmallow.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marshmallow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ImplementPersistence(this 
        IServiceCollection services, 
        IConfiguration configuration)
    {
        var databaseConfiguration = configuration
            .GetRequiredSection(DatabaseConfiguration.SectionName)
            .Get<DatabaseConfiguration>() ?? throw new ArgumentException("");

        if (databaseConfiguration.Provider is DatabaseProvider.Sqlite)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();

            connectionStringBuilder.DataSource = databaseConfiguration.DataSource;

            services.AddDbContext<DatabaseContext>(
                e => e.UseSqlite(connectionStringBuilder.ToString()));
        }

        services.AddHostedService<MigrationsJob>();

        services.AddScoped<PublishDomainEventsInterceptor>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGroupsRepository, GroupsRepository>();
        services.AddScoped<IConsumersRepository, ConsumersRepository>();

        services.AddMemoryCache();
        services.AddScoped<TopicsRepository>();
        services.AddScoped<ITopicsRepository, CachedTopicsRepository>();

        return services;
    }
}
