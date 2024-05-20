using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;

namespace Marshmallow.Endpoints;

public static class DependencyInjection
{
    public static IServiceCollection ImplementEndpoints(this
        IServiceCollection services)
    {
        services.AddFastEndpoints();
        
        return services;
    }
}
