namespace EduHub.Ch02.Domain;

/// <summary>Sự kiện của CLB, kèm danh sách đăng ký và check-in.</summary>
public class Event
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public required string Title { get; set; }
    public DateTime Date { get; set; }

    public HashSet<Guid> RegisteredMemberIds { get; } = new();
    public HashSet<Guid> CheckedInMemberIds { get; } = new();

    // Tỉ lệ tham dự = số check-in / số đăng ký; 0 nếu chưa ai đăng ký.
    public double AttendanceRate =>
        RegisteredMemberIds.Count == 0
            ? 0
            : (double)CheckedInMemberIds.Count / RegisteredMemberIds.Count;

    // Đăng ký tham dự; HashSet.Add trả về false nếu thành viên đã đăng ký trước đó.
    public bool Register(Guid memberId)
    {
        return RegisteredMemberIds.Add(memberId);
    }

    // Chỉ thành viên đã đăng ký mới được check-in.
    public void CheckIn(Guid memberId)
    {
        if (!RegisteredMemberIds.Contains(memberId))
            throw new InvalidOperationException("Thành viên chưa đăng ký sự kiện");

        CheckedInMemberIds.Add(memberId);
    }
}
