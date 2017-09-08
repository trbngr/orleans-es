namespace Common
{
    public interface INotification
    {
        string Subject { get; set; }
        string Body { get; set; }
    }
}