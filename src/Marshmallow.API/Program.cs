using FastEndpoints;
using Marshmallow.Application;
using Marshmallow.Application.Services;
using Marshmallow.Endpoints;
using Marshmallow.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(request => request.ToString());

    options.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "Swagger Demo Documentation",
                Version = "v1",
                Description = "This is a demo to see how documentation can easily be generated for ASP.NET Core Web APIs using Swagger and ReDoc.",
                Contact = new OpenApiContact
                {
                    Name = "Christian Schou",
                    Email = "someemail@somedomain.com"
                }
            });
});
builder.Services.ImplementEndpoints();
builder.Services.ImplementApplication();
builder.Services.ImplementPersistence(builder.Configuration);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(e =>
{
    e.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    e.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseReDoc(c =>
    {
        c.DocumentTitle = "REDOC API Documentation";
        c.SpecUrl = "/swagger/v1/swagger.json";
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseFastEndpoints();
app.MapGrpcService<TopicService>();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();
