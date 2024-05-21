using Marshmallow.Application.Hubs;
using Marshmallow.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Marshmallow.Application.Notifications
{
    internal class NotificationsBackgroundService(
        IServiceProvider serviceProvider,
        ITopicsHub topicsHub) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<ITopicsRepository>();
            var topic = await repository.GetByNameAsync("test");

            while (true)
            {
                await topicsHub.PublishIntoTopicAsync(topic, new Protos.Types.MessageProto()
                {
                    Offset = 0,
                    Payload = new Protos.Types.PayloadProto
                    {
                        Value = Google.Protobuf.ByteString.CopyFromUtf8("test")
                    }
                });

                await Task.Delay(50);
            }
        }
    }
}
