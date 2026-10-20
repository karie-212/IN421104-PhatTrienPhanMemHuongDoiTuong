using EduHub.Ch02.Notifications;

namespace Ch02.Refactor.Tests;

// Test cho các design pattern (Factory, ...), theo mẫu AAA.
public class PatternTests
{
    [Theory]
    [InlineData(NotificationChannel.Email, typeof(EmailNotificationService))]
    [InlineData(NotificationChannel.Sms, typeof(SmsNotificationService))]
    [InlineData(NotificationChannel.Slack, typeof(SlackNotificationService))]
    public void NotificationServiceFactory_Create_ReturnsServiceForChannel(NotificationChannel channel, Type expectedType)
    {
        // Arrange
        var factory = new NotificationServiceFactory();

        // Act
        var service = factory.Create(channel);

        // Assert
        Assert.IsType(expectedType, service);
    }

    [Fact]
    public void NotificationServiceFactory_Create_UnsupportedChannel_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var factory = new NotificationServiceFactory();
        var unsupported = (NotificationChannel)99;

        // Act
        var act = () => factory.Create(unsupported);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }
}
