using Common;

namespace Grains
{
    public class Notification : INotification
    {
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}