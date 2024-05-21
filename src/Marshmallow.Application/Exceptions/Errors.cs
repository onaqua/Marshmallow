using Grpc.Core;

namespace Marshmallow.Application.Exceptions;

public static class Errors
{
    public static class Topic
    {
        public static RpcException NotFound(string topicName) =>
            new RpcException(new Status(StatusCode.NotFound, $"Topic with {topicName} name not found."));
    }

    public static class Consumer
    {
        public static RpcException NotFound(Guid consumerId) =>
               new RpcException(new Status(StatusCode.NotFound, $"Consumer with {consumerId} id not found."));
    }
}