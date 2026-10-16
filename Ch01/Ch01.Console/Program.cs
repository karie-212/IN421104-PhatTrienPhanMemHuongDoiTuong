using EduHub.Ch01.Domain;

const string Separator = "========================================";
const int EventCapacity = 50;
const string DateFormat = "dd/MM/yyyy HH:mm";
const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
const string MethodQrCode = "QrCode";
const string MethodManual = "Manual";
const string ExpectedErrorPrefix = "Bắt lỗi dự kiến: ";

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("=== EDUHUB ===");
Console.WriteLine();

try
{
    // Tạo câu lạc bộ mới
    var club = Club.Create("CLB Lập trình DThU", "Nơi chia sẻ kiến thức và kỹ năng phát triển phần mềm.");
    Console.WriteLine("[1. Tạo CLB]");
    Console.WriteLine("Tên CLB: " + club.Name);
    Console.WriteLine("Mô tả: " + club.Description);
    Console.WriteLine("ID: " + club.Id);
    Console.WriteLine();

    // Khởi tạo và thêm các thành viên vào câu lạc bộ
    var member1 = Member.Create("Trần Thái Kiệt", "0024418950@student.dthu.edu.vn", "0024418950");
    var member2 = Member.Create("Nguyễn Văn A", "nva@student.dthu.edu.vn", "0024418951");

    club.AddMember(member1);
    club.AddMember(member2);

    Console.WriteLine("[2. Thêm thành viên]");
    Console.WriteLine("Số lượng thành viên: " + club.MemberCount());
    foreach (var m in club.Members)
    {
        Console.WriteLine("- " + m + " | Email: " + m.Email + " | Active: " + m.IsActive());
    }
    Console.WriteLine();

    // Tạo sự kiện mới cho câu lạc bộ
    var eventDate = DateTime.Now.AddDays(7);
    var ev = Event.Create(club.Id, "Workshop: Domain-Driven Design in C#", eventDate, capacity: EventCapacity, isOnline: true);

    string hinhThuc;
    if (ev.IsOnline)
    {
        hinhThuc = "Online";
    }
    else
    {
        hinhThuc = "Offline";
    }

    string conCho;
    if (ev.HasCapacity(0))
    {
        conCho = "Còn chỗ";
    }
    else
    {
        conCho = "Hết chỗ";
    }

    Console.WriteLine("[3. Tạo Sự kiện]");
    Console.WriteLine("Sự kiện: " + ev.Title);
    Console.WriteLine("Ngày diễn ra: " + ev.Date.ToString(DateFormat));
    Console.WriteLine("Sức chứa: " + ev.Capacity + " người | Hình thức: " + hinhThuc);
    Console.WriteLine("Kiểm tra sức chứa: " + conCho);
    Console.WriteLine();

    // Đăng ký tham gia sự kiện (mặc định trạng thái Pending)
    var reg1 = Registration.Create(member1.Id, ev.Id);
    Console.WriteLine("[4. Đăng ký sự kiện]");
    Console.WriteLine("Lượt đăng ký của " + member1.Name + ": Status = '" + reg1.Status + "'");

    // Xác nhận lượt đăng ký (chuyển sang Confirmed)
    reg1.Confirm();
    Console.WriteLine("Sau khi xác nhận: Status = '" + reg1.Status + "'");
    Console.WriteLine();

    // Thực hiện điểm danh thành viên
    var checkIn = CheckIn.Create(reg1, MethodQrCode);
    Console.WriteLine("[5. Điểm danh / Check-in]");
    Console.WriteLine(checkIn.Summary());
    Console.WriteLine();

    // Xuất báo cáo tham dự
    Console.WriteLine(Separator);
    Console.WriteLine("BÁO CÁO THAM DỰ SỰ KIỆN");
    Console.WriteLine(Separator);
    Console.WriteLine("CLB          : " + club.Name);
    Console.WriteLine("Sự kiện      : " + ev.Title);
    Console.WriteLine("Thành viên   : " + member1);
    Console.WriteLine("Trạng thái   : " + reg1.Status);
    Console.WriteLine("Phương thức  : " + checkIn.Method);
    Console.WriteLine("Thời gian C/I: " + checkIn.CheckedInAt.ToString(DateTimeFormat));
    Console.WriteLine();

    // Kiểm tra xử lý ngoại lệ khi vi phạm quy tắc nghiệp vụ
    Console.WriteLine("[6. Kiểm tra rào cản nghiệp vụ]");
    var reg2 = Registration.Create(member2.Id, ev.Id);

    // 6a: điểm danh cho phiếu chưa confirm phải bị chặn
    try
    {
        CheckIn.Create(reg2, MethodManual);
        Console.WriteLine("SAI: domain không chặn check-in khi phiếu chưa xác nhận!");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ExpectedErrorPrefix + ex.Message);
    }

    // 6b: Cancelled là trạng thái cuối, không được confirm lại
    reg2.Cancel();
    try
    {
        reg2.Confirm();
        Console.WriteLine("SAI: domain cho phép confirm phiếu đã hủy!");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ExpectedErrorPrefix + ex.Message);
    }
}
catch (Exception ex)
{
    Console.WriteLine("Lỗi không mong muốn: " + ex.Message);
}

Console.WriteLine();