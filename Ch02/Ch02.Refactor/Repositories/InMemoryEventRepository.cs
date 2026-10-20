using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

/// <summary>Lưu sự kiện trong bộ nhớ.</summary>
public class InMemoryEventRepository : IEventRepository
{
    private readonly List<Event> _events = new();

    public IReadOnlyList<Event> GetByClub(Guid clubId)
    {
        return _events.Where(e => e.ClubId == clubId).ToList();
    }

    public Event? GetById(Guid id)
    {
        return _events.FirstOrDefault(e => e.Id == id);
    }

    public void Add(Event ev)
    {
        _events.Add(ev);
    }
}
