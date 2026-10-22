using EduHub.Ch02.Logging;
using EduHub.Ch02.Notifications;
using EduHub.Ch02.Patterns.Observers;
using EduHub.Ch02.Patterns.Strategies;
using EduHub.Ch02.Repositories;
using EduHub.Ch02.Services;

// ═══════════════════════════════════════════════════════════════
//  EduHub Ch02 — Refactoring với SOLID và Design Patterns
//  Composition root: nơi DUY NHẤT new các class cụ thể và nối chúng lại.
//  Mọi class khác chỉ nhận interface qua constructor (DIP).
// ═══════════════════════════════════════════════════════════════

IAppLogger logger = new ConsoleLogger();
INotificationServiceFactory notificationFactory = new NotificationServiceFactory();
INotificationService notifier = notificationFactory.Create(NotificationChannel.Email);

// Decorator: bọc repository gốc để ghi log mỗi lần truy cập.
IClubRepository clubRepo = new LoggingClubRepository(new InMemoryClubRepository(), logger);
IMemberRepository memberRepo = new InMemoryMemberRepository();
IEventRepository eventRepo = new InMemoryEventRepository();

IClubService clubService = new ClubService(clubRepo, memberRepo, notifier, logger);
IEventService eventService = new EventService(clubRepo, memberRepo, eventRepo);
IReportService reportService = new ReportService(clubRepo, memberRepo, eventRepo);

// (1) Tạo CLB và thêm thành viên
Console.WriteLine("=== (1) Tạo CLB và thêm thành viên ===");
var club = clubService.CreateClub("Tech Club", "Câu lạc bộ công nghệ", "president@gmail.com");
var memberA = clubService.AddMember(club.Id, "Nguyễn Văn A", "a@student.dthu.edu.vn");
var memberB = clubService.AddMember(club.Id, "Trần Thị B", "b@student.dthu.edu.vn");

// (2) Observer: EventService báo cho các observer khi có sự kiện mới
Console.WriteLine("\n=== (2) Observer: tạo sự kiện, đăng ký, check-in ===");
eventService.Subscribe(new LogNotifier(logger));
eventService.Subscribe(new EmailNotifier(memberRepo, notifier));
var workshop = eventService.CreateEvent(club.Id, "Workshop SOLID", new DateTime(2026, 11, 20));
eventService.Register(workshop.Id, memberA.Id);
eventService.Register(workshop.Id, memberB.Id);
eventService.CheckIn(workshop.Id, memberA.Id);
Console.WriteLine("  Đã đăng ký 2 thành viên, check-in 1; tỉ lệ tham dự: " + workshop.AttendanceRate.ToString("P0"));

// (3) Strategy: cùng dữ liệu, đổi strategy là đổi định dạng
Console.WriteLine("\n=== (3) Strategy: báo cáo Console ===");
Console.Write(reportService.Generate(new ConsoleReportStrategy()));
Console.WriteLine("\n=== (3) Strategy: báo cáo CSV ===");
Console.Write(reportService.Generate(new CsvReportStrategy()));

// (4) Factory: tạo INotificationService theo từng kênh
Console.WriteLine("\n=== (4) Factory: gửi thử qua mọi kênh thông báo ===");
foreach (var channel in Enum.GetValues<NotificationChannel>())
    notificationFactory.Create(channel).Send("admin@eduhub.vn", "Thông báo thử qua kênh " + channel);

// (5) Validate: luật đặt tên nằm ở Domain (Club.Create)
Console.WriteLine("\n=== (5) Validate: tạo CLB tên rỗng ===");
try
{
    clubService.CreateClub("", "CLB không tên", "president@gmail.com");
}
catch (ArgumentException ex)
{
    Console.WriteLine("  Lỗi: " + ex.Message);
}
