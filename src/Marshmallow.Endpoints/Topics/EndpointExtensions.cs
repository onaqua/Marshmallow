using ErrorOr;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Marshmallow.Endpoints;

public static class EndpointExtensions
{
    public static async Task<TResponse?> SendFailureAsync<TRequest, TResponse>(this
        Endpoint<TRequest, TResponse> endpoint,
        List<Error> errors,
        int statusCode,
        CancellationToken cancellationToken = default) 
        where TRequest : notnull
    {
        endpoint.HttpContext.Response.StatusCode = statusCode;
        
        await endpoint.HttpContext.Response.WriteAsJsonAsync(
            errors, cancellationToken);

        return default(TResponse);
    }
}
