namespace EduHub.Ch02.Notifications;

/// <summary>Gửi thông báo qua một kênh cụ thể.</summary>
public interface INotificationService
{
    void Send(string to, string message);
}
