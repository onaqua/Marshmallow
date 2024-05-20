using Grpc.Core;
using Marshmallow.Shared.Constants;

namespace Marshmallow.Extensions.Extensions;

public static class GrpcExtensions
{
    public static Guid GetConsumerId(this ServerCallContext context) => 
        Guid.TryParse(context
            .GetHttpContext().User
            .FindFirst(InternalClaims.ConsumerId)?.Value, out var id) ? id : throw new InvalidOperationException("ConsumerId not found");

    public static Guid GetProducerId(this ServerCallContext context) =>
        Guid.TryParse(context
            .GetHttpContext().User
            .FindFirst(InternalClaims.ProducerId)?.Value, out var id) ? id : throw new InvalidOperationException("ProducerId not found");
}
