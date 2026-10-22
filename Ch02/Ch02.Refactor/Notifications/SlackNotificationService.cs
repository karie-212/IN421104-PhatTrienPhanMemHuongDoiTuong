namespace EduHub.Ch02.Notifications;

/// <summary>Gửi thông báo qua Slack.</summary>
public class SlackNotificationService : INotificationService
{
    public void Send(string to, string message)
    {
        Console.WriteLine("  [SLACK] gửi tới " + to + ": " + message);
    }
}
