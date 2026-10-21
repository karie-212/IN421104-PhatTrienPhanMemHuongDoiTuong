namespace EduHub.Ch02.Patterns.Strategies;

/// <summary>Strategy: quyết định cách định dạng báo cáo từ danh sách ClubStat.</summary>
public interface IReportStrategy
{
    string Render(IReadOnlyList<ClubStat> stats);
}
