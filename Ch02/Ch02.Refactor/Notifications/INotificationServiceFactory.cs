namespace EduHub.Ch02.Notifications;

/// <summary>Tạo INotificationService phù hợp với kênh được chọn.</summary>
public interface INotificationServiceFactory
{
    INotificationService Create(NotificationChannel channel);
}
