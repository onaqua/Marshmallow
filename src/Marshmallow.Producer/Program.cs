using Grpc.Net.Client;
using Marshmallow.Protos.Services;

var httpHandler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

var channel = GrpcChannel.ForAddress(new Uri("https://marshmallow.api:8081"), new GrpcChannelOptions { HttpHandler = httpHandler });
var client = new TopicService.TopicServiceClient(channel);

while (true)
{
    try
    {

        var message = client.Subscribe(new SubscribeProtoRequest() { GroupName = "test", TopicName = "test" });

        while (await message.ResponseStream.MoveNext(CancellationToken.None))
        {
            var response = message.ResponseStream.Current;

            Console.WriteLine(response.Payload);
        }
    }
    catch { await Task.Delay(1000); }
}