using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

/// <summary>Lưu trữ và truy vấn sự kiện.</summary>
public interface IEventRepository
{
    IReadOnlyList<Event> GetByClub(Guid clubId);
    Event? GetById(Guid id);
    void Add(Event ev);
}
