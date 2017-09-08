using System.Threading.Tasks;
using Common;
using Orleans;
using Orleans.Runtime;
using Orleans.Streams;

namespace Grains
{
    [ImplicitStreamSubscription(Streams.Notifications)]
    public class NotificationReceiver : Grain, INotificationReceiver
    {
        public override async Task OnActivateAsync()
        {
            var id = this.GetPrimaryKey();
            var provider = GetStreamProvider("SMSProvider");
            var stream = provider.GetStream<INotification>(id, Streams.Notifications);
            await stream.SubscribeAsync(Send);
        }

        Task Send(INotification notification, StreamSequenceToken token)
        {
            var logger = GetLogger();
            logger.Info($"Send notification: {notification.Subject}. {token}");
            return Receive(notification);
        }

        public Task Receive(INotification notification)
        {
            return Task.CompletedTask;
        }
    }
}