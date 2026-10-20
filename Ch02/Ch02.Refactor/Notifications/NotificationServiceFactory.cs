namespace EduHub.Ch02.Notifications;

/// <summary>
/// Factory chọn INotificationService theo NotificationChannel.
/// Chuỗi if/else Gmail/Yahoo cũ trong ClubManager thực chất chỉ là một kênh Email
/// (chỉ khác tên miền người nhận), nên được gộp thành EmailNotificationService.
/// Factory chọn theo kênh, không theo địa chỉ; nơi gọi chỉ biết INotificationService.
/// </summary>
public class NotificationServiceFactory : INotificationServiceFactory
{
    public INotificationService Create(NotificationChannel channel)
    {
        if (channel == NotificationChannel.Email)
            return new EmailNotificationService();
        else if (channel == NotificationChannel.Sms)
            return new SmsNotificationService();
        else if (channel == NotificationChannel.Slack)
            return new SlackNotificationService();
        else
            throw new ArgumentOutOfRangeException(nameof(channel), channel, "Kênh thông báo không được hỗ trợ");
    }
}
