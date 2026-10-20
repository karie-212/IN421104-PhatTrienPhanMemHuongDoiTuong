using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

/// <summary>Lưu CLB trong bộ nhớ.</summary>
public class InMemoryClubRepository : IClubRepository
{
    private readonly List<Club> _clubs = new();

    public IReadOnlyList<Club> GetAll()
    {
        return _clubs.AsReadOnly();
    }

    public Club? GetById(Guid id)
    {
        return _clubs.FirstOrDefault(c => c.Id == id);
    }

    public void Add(Club club)
    {
        _clubs.Add(club);
    }
}
