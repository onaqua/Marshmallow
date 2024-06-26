﻿using Marshmallow.Core.Entities;

namespace Marshmallow.Application.Authorization;

public interface IAuthorizationService
{
    public Task<string> AuthorizeConsumerAsync(Consumer consumer, CancellationToken cancellationToken = default);
    public Task<string> AuthorizeProducerAsync(Producer producer, CancellationToken cancellationToken = default);
}

public class AuthorizationService(IJwtTokenProvider jwtTokenProvider) : IAuthorizationService
{
    public async Task<string> AuthorizeConsumerAsync(Consumer consumer, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => jwtTokenProvider.CreateConsumerToken(consumer), cancellationToken);
    }

    public async Task<string> AuthorizeProducerAsync(Producer producer, CancellationToken cancellationToken = default)
    { 
        return await Task.Run(() => jwtTokenProvider.CreateProducerToken(producer), cancellationToken);
    }
}