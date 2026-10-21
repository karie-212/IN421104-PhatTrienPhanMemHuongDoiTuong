using System.Text;

namespace EduHub.Ch02.Patterns.Strategies;

/// <summary>Báo cáo dạng văn bản dễ đọc: mỗi CLB một dòng, nhãn tiếng Việt.</summary>
public class ConsoleReportStrategy : IReportStrategy
{
    public string Render(IReadOnlyList<ClubStat> stats)
    {
        var sb = new StringBuilder();

        foreach (var s in stats)
            sb.AppendLine("CLB: " + s.ClubName
                + " | Thành viên: " + s.Members
                + " | Sự kiện: " + s.Events
                + " | Đăng ký: " + s.Registered
                + " | Check-in: " + s.CheckedIn);

        return sb.ToString();
    }
}
