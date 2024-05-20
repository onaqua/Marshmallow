using Marshmallow.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Marshmallow.Application;

public static class DependencyInjection
{
    public static IServiceCollection ImplementApplication(this 
        IServiceCollection services)
    {
        services.AddMediatR(e => e.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddGrpc();

        services.AddSingleton<IStreamClients, StreamClients>();

        services.AddHostedService<Notifier>();

        return services;
    }
}
