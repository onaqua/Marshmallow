using System.Text;
using Marshmallow.Application.Authorization;
using Marshmallow.Application.Hubs;
using Marshmallow.Application.Notifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Marshmallow.Application;

public static class DependencyInjection
{
    public static IServiceCollection ImplementApplication(this
        IServiceCollection services)
    {
        services.AddMediatR(e => e.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddGrpc();

        services.AddHostedService<NotificationsBackgroundService>();
        services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddSingleton<ITopicsHub, TopicsHub>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456test1524312345678901234")),
                        ValidateActor = false,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = false,
                        ValidateLifetime = false,
                    };
                });

        return services;
    }
}
