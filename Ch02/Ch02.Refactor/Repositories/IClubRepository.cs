using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

/// <summary>Lưu trữ và truy vấn CLB.</summary>
public interface IClubRepository
{
    IReadOnlyList<Club> GetAll();
    Club? GetById(Guid id);
    void Add(Club club);
}
