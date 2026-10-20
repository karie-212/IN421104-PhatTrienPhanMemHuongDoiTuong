using EduHub.Ch02.Domain;
using EduHub.Ch02.Logging;
using EduHub.Ch02.Notifications;
using EduHub.Ch02.Repositories;

namespace Ch02.Refactor.Tests;

// Test cho các design pattern (Factory, Decorator, ...), theo mẫu AAA.
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

    [Fact]
    public void LoggingClubRepository_Add_LogsClubNameAndDelegatesToInner()
    {
        // Arrange
        var inner = new InMemoryClubRepository();
        var logger = new FakeLogger();
        var repository = new LoggingClubRepository(inner, logger);
        var club = Club.Create("CLB Lập trình", "Sinh hoạt lập trình mỗi thứ Bảy");

        // Act
        repository.Add(club);

        // Assert
        Assert.Contains(logger.Messages, m => m.Contains("CLB Lập trình"));
        Assert.Same(club, inner.GetById(club.Id));
    }

    // Logger giả: lưu message vào danh sách để test kiểm tra, không in ra đâu cả.
    private class FakeLogger : IAppLogger
    {
        public List<string> Messages { get; } = new();

        public void Info(string message)
        {
            Messages.Add(message);
        }
    }
}
