namespace EduHub.Ch02.Notifications;

/// <summary>Gửi thông báo qua email.</summary>
public class EmailNotificationService : INotificationService
{
    public void Send(string to, string message)
    {
        Console.WriteLine("  [EMAIL] gửi tới " + to + ": " + message);
    }
}
