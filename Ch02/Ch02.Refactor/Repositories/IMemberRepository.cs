using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

/// <summary>Lưu trữ và truy vấn thành viên.</summary>
public interface IMemberRepository
{
    IReadOnlyList<Member> GetByClub(Guid clubId);
    Member? GetById(Guid id);
    void Add(Member member);
}
