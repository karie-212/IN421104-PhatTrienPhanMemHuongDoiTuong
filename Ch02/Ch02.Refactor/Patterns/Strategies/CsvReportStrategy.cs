using System.Text;

namespace EduHub.Ch02.Patterns.Strategies;

/// <summary>Báo cáo dạng CSV: một dòng tiêu đề, sau đó mỗi CLB một dòng.</summary>
public class CsvReportStrategy : IReportStrategy
{
    public string Render(IReadOnlyList<ClubStat> stats)
    {
        var sb = new StringBuilder();
        sb.AppendLine("ClubName,Members,Events,Registered,CheckedIn");

        // Tên CLB đặt trong ngoặc kép (nhân đôi dấu " bên trong) để dấu phẩy trong tên không làm lệch cột.
        foreach (var s in stats)
            sb.AppendLine("\"" + s.ClubName.Replace("\"", "\"\"") + "\","
                + s.Members + "," + s.Events + "," + s.Registered + "," + s.CheckedIn);

        return sb.ToString();
    }
}
