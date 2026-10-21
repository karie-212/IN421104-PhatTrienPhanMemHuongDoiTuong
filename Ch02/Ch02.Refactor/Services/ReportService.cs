using EduHub.Ch02.Patterns.Strategies;
using EduHub.Ch02.Repositories;

namespace EduHub.Ch02.Services;

/// <summary>
/// Context trong Strategy: tính số liệu từng CLB rồi giao cho strategy định dạng.
/// ReportService không in ra Console; nơi gọi tự quyết định in, ghi file hay gửi đi.
/// Thêm định dạng mới chỉ cần thêm IReportStrategy, không sửa class này (OCP).
/// </summary>
public class ReportService : IReportService
{
    private readonly IClubRepository _clubs;
    private readonly IMemberRepository _members;
    private readonly IEventRepository _events;

    public ReportService(IClubRepository clubs, IMemberRepository members, IEventRepository events)
    {
        _clubs = clubs;
        _members = members;
        _events = events;
    }

    public string Generate(IReportStrategy strategy)
    {
        var stats = new List<ClubStat>();

        foreach (var club in _clubs.GetAll())
        {
            var events = _events.GetByClub(club.Id);

            stats.Add(new ClubStat(
                club.Name,
                _members.GetByClub(club.Id).Count,
                events.Count,
                events.Sum(e => e.RegisteredMemberIds.Count),
                events.Sum(e => e.CheckedInMemberIds.Count)));
        }

        return strategy.Render(stats);
    }
}
