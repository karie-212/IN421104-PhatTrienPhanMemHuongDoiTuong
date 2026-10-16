using EduHub.Ch01.Domain;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("=== EDUHUB ===");
Console.WriteLine();

try
{
    // Tạo câu lạc bộ mới
    var club = Club.Create("CLB Lập trình DThU", "Nơi chia sẻ kiến thức và kỹ năng phát triển phần mềm.");
    Console.WriteLine("[1. Tạo CLB]");
    Console.WriteLine($"Tên CLB: {club.Name}");
    Console.WriteLine($"Mô tả: {club.Description}");
    Console.WriteLine($"ID: {club.Id}");
    Console.WriteLine();

    // Khởi tạo và thêm các thành viên vào câu lạc bộ
    var member1 = Member.Create("Trần Thái Kiệt", "0024418950@student.dthu.edu.vn", "0024418950");
    var member2 = Member.Create("Nguyễn Văn A", "nva@student.dthu.edu.vn", "0024418951");

    club.AddMember(member1);
    club.AddMember(member2);

    Console.WriteLine("[2. Thêm thành viên]");
    Console.WriteLine($"Số lượng thành viên: {club.MemberCount()}");
    foreach (var m in club.Members)
    {
        Console.WriteLine($"- {m} | Email: {m.Email} | Active: {m.IsActive()}");
    }
    Console.WriteLine();

    // Tạo sự kiện mới cho câu lạc bộ
    var eventDate = DateTime.Now.AddDays(7);
    var ev = Event.Create(club.Id, "Workshop: Domain-Driven Design in C#", eventDate, capacity: 50, isOnline: true);

    Console.WriteLine("[3. Tạo Sự kiện]");
    Console.WriteLine($"Sự kiện: {ev.Title}");
    Console.WriteLine($"Ngày diễn ra: {ev.Date:dd/MM/yyyy HH:mm}");
    Console.WriteLine($"Sức chứa: {ev.Capacity} người | Hình thức: {(ev.IsOnline ? "Online" : "Offline")}");
    Console.WriteLine($"Kiểm tra sức chứa: {(ev.HasCapacity(0) ? "Còn chỗ" : "Hết chỗ")}");
    Console.WriteLine();

    // Đăng ký tham gia sự kiện (mặc định trạng thái Pending)
    var reg1 = Registration.Create(member1.Id, ev.Id);
    Console.WriteLine("[4. Đăng ký sự kiện]");
    Console.WriteLine($"Lượt đăng ký của {member1.Name}: Status = '{reg1.Status}'");

    // Xác nhận lượt đăng ký (chuyển sang Confirmed)
    reg1.Confirm();
    Console.WriteLine($"Sau khi xác nhận: Status = '{reg1.Status}'");
    Console.WriteLine();

    // Thực hiện điểm danh thành viên
    var checkIn = CheckIn.Create(reg1, "QrCode");
    Console.WriteLine("[5. Điểm danh / Check-in]");
    Console.WriteLine(checkIn.Summary());
    Console.WriteLine();

    // Xuất báo cáo tham dự
    Console.WriteLine("==== BÁO CÁO THAM DỰ SỰ KIỆN ====");
    Console.WriteLine($"CLB          : {club.Name}");
    Console.WriteLine($"Sự kiện      : {ev.Title}");
    Console.WriteLine($"Thành viên   : {member1}");
    Console.WriteLine($"Trạng thái   : {reg1.Status}");
    Console.WriteLine($"Phương thức  : {checkIn.Method}");
    Console.WriteLine($"Thời gian C/I: {checkIn.CheckedInAt:dd/MM/yyyy HH:mm:ss}");
    Console.WriteLine();

    // Kiểm tra xử lý ngoại lệ khi vi phạm quy tắc nghiệp vụ
    Console.WriteLine("[6. Kiểm tra rào cản nghiệp vụ]");
    var reg2 = Registration.Create(member2.Id, ev.Id);
    try
    {
        // Thử điểm danh cho phiếu chưa được confirm
        var invalidCheckIn = CheckIn.Create(reg2, "Manual");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Bắt lỗi dự kiến: {ex.Message}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Lỗi không mong muốn: {ex.Message}");
}

Console.WriteLine();