namespace EduHub.Ch02.Notifications;

/// <summary>Gửi thông báo qua SMS.</summary>
public class SmsNotificationService : INotificationService
{
    public void Send(string to, string message)
    {
        Console.WriteLine("  [SMS] gửi tới " + to + ": " + message);
    }
}
