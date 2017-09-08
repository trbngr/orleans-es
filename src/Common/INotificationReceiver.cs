using System.Threading.Tasks;

namespace Common
{
    public interface INotificationReceiver
    {
        Task Receive(INotification notification);
    }
}