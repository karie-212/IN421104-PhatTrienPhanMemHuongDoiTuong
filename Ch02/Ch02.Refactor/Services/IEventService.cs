using EduHub.Ch02.Domain;
using EduHub.Ch02.Patterns.Observers;

namespace EduHub.Ch02.Services;

/// <summary>Nghiệp vụ sự kiện: tạo, đăng ký, check-in; báo cho observer khi có sự kiện mới.</summary>
public interface IEventService
{
    void Subscribe(IEventObserver o);
    void Unsubscribe(IEventObserver o);
    Event CreateEvent(Guid clubId, string title, DateTime date);
    void Register(Guid eventId, Guid memberId);
    void CheckIn(Guid eventId, Guid memberId);
}
