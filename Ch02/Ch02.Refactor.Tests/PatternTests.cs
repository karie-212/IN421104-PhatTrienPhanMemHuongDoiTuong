using EduHub.Ch02.Domain;
using EduHub.Ch02.Logging;
using EduHub.Ch02.Notifications;
using EduHub.Ch02.Patterns.Observers;
using EduHub.Ch02.Patterns.Strategies;
using EduHub.Ch02.Repositories;
using EduHub.Ch02.Services;

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

    [Fact]
    public void EventService_CreateEvent_NotifiesEachObserverOnce()
    {
        // Arrange
        var clubs = new InMemoryClubRepository();
        var club = Club.Create("CLB Lập trình", "Sinh hoạt lập trình mỗi thứ Bảy");
        clubs.Add(club);
        var service = new EventService(clubs, new InMemoryMemberRepository(), new InMemoryEventRepository());
        var first = new SpyObserver();
        var second = new SpyObserver();
        service.Subscribe(first);
        service.Subscribe(second);

        // Act
        service.CreateEvent(club.Id, "Workshop Git", DateTime.Today.AddDays(7));

        // Assert
        Assert.Equal(1, first.CallCount);
        Assert.Equal(1, second.CallCount);
    }

    [Fact]
    public void EventService_CreateEvent_AfterUnsubscribe_DoesNotNotifyObserver()
    {
        // Arrange
        var clubs = new InMemoryClubRepository();
        var club = Club.Create("CLB Lập trình", "Sinh hoạt lập trình mỗi thứ Bảy");
        clubs.Add(club);
        var service = new EventService(clubs, new InMemoryMemberRepository(), new InMemoryEventRepository());
        var observer = new SpyObserver();
        service.Subscribe(observer);
        service.Unsubscribe(observer);

        // Act
        service.CreateEvent(club.Id, "Workshop Git", DateTime.Today.AddDays(7));

        // Assert
        Assert.Equal(0, observer.CallCount);
    }

    [Fact]
    public void ReportStrategies_RenderSameStats_ProduceDifferentFormats()
    {
        // Arrange
        var stats = new List<ClubStat>
        {
            new("CLB Lập trình", 12, 3, 20, 15),
            new("CLB Tiếng Anh", 8, 1, 6, 4)
        };
        var console = new ConsoleReportStrategy();
        var csv = new CsvReportStrategy();

        // Act
        var consoleOutput = console.Render(stats);
        var csvOutput = csv.Render(stats);

        // Assert
        Assert.NotEqual(consoleOutput, csvOutput);
        Assert.Contains("CLB: CLB Lập trình | Thành viên: 12 | Sự kiện: 3 | Đăng ký: 20 | Check-in: 15", consoleOutput);
        var csvLines = csvOutput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(3, csvLines.Length);
        Assert.Equal("ClubName,Members,Events,Registered,CheckedIn", csvLines[0]);
        Assert.Equal("\"CLB Lập trình\",12,3,20,15", csvLines[1]);
    }

    [Fact]
    public void ReportService_Generate_PassesComputedStatsToStrategy()
    {
        // Arrange
        var clubs = new InMemoryClubRepository();
        var members = new InMemoryMemberRepository();
        var events = new InMemoryEventRepository();
        var club = Club.Create("CLB Lập trình", "Sinh hoạt lập trình mỗi thứ Bảy");
        clubs.Add(club);
        var member = new Member { Id = Guid.NewGuid(), Name = "Nguyễn Văn A", Email = "a@student.dthu.edu.vn", ClubId = club.Id };
        members.Add(member);
        var ev = new Event { Id = Guid.NewGuid(), ClubId = club.Id, Title = "Workshop Git", Date = DateTime.Today };
        ev.Register(member.Id);
        ev.CheckIn(member.Id);
        events.Add(ev);
        var service = new ReportService(clubs, members, events);
        var spy = new SpyReportStrategy();

        // Act
        var output = service.Generate(spy);

        // Assert
        Assert.Equal("spy", output);
        Assert.Equal(new ClubStat("CLB Lập trình", 1, 1, 1, 1), Assert.Single(spy.Received!));
    }

    // Strategy giả: lưu lại danh sách ClubStat nhận được.
    private class SpyReportStrategy : IReportStrategy
    {
        public IReadOnlyList<ClubStat>? Received { get; private set; }

        public string Render(IReadOnlyList<ClubStat> stats)
        {
            Received = stats;
            return "spy";
        }
    }

    // Observer giả: chỉ đếm số lần được báo.
    private class SpyObserver : IEventObserver
    {
        public int CallCount { get; private set; }

        public void OnEventCreated(Event ev, Club club)
        {
            CallCount++;
        }
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
