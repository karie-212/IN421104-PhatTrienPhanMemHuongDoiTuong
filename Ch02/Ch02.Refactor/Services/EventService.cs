using EduHub.Ch02.Domain;
using EduHub.Ch02.Patterns.Observers;
using EduHub.Ch02.Repositories;

namespace EduHub.Ch02.Services;

/// <summary>
/// Subject trong Observer: tạo sự kiện rồi báo cho các observer đã đăng ký.
/// EventService không tự gửi email hay ghi log; việc đó do observer đảm nhận,
/// nên thêm kênh mới chỉ cần thêm observer, không sửa class này (OCP).
/// </summary>
public class EventService : IEventService
{
    private readonly IClubRepository _clubs;
    private readonly IMemberRepository _members;
    private readonly IEventRepository _events;
    private readonly List<IEventObserver> _observers = new();

    public EventService(IClubRepository clubs, IMemberRepository members, IEventRepository events)
    {
        _clubs = clubs;
        _members = members;
        _events = events;
    }

    public void Subscribe(IEventObserver o)
    {
        _observers.Add(o);
    }

    public void Unsubscribe(IEventObserver o)
    {
        _observers.Remove(o);
    }

    public Event CreateEvent(Guid clubId, string title, DateTime date)
    {
        var club = _clubs.GetById(clubId)
            ?? throw new InvalidOperationException("Không tìm thấy CLB");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tên sự kiện không được để trống");

        var ev = new Event { Id = Guid.NewGuid(), ClubId = clubId, Title = title, Date = date };
        _events.Add(ev);

        // Duyệt bản sao để observer có thể Unsubscribe ngay trong OnEventCreated.
        foreach (var observer in _observers.ToList())
            observer.OnEventCreated(ev, club);

        return ev;
    }

    public void Register(Guid eventId, Guid memberId)
    {
        var ev = _events.GetById(eventId)
            ?? throw new InvalidOperationException("Không tìm thấy sự kiện");
        var member = _members.GetById(memberId)
            ?? throw new InvalidOperationException("Không tìm thấy thành viên");

        if (member.ClubId != ev.ClubId)
            throw new InvalidOperationException("Thành viên không thuộc CLB tổ chức sự kiện");

        ev.Register(memberId);
    }

    public void CheckIn(Guid eventId, Guid memberId)
    {
        var ev = _events.GetById(eventId)
            ?? throw new InvalidOperationException("Không tìm thấy sự kiện");

        // Luật "phải đăng ký trước" nằm ở Domain.
        ev.CheckIn(memberId);
    }
}
