using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

/// <summary>Lưu thành viên trong bộ nhớ.</summary>
public class InMemoryMemberRepository : IMemberRepository
{
    private readonly List<Member> _members = new();

    public IReadOnlyList<Member> GetByClub(Guid clubId)
    {
        return _members.Where(m => m.ClubId == clubId).ToList();
    }

    public Member? GetById(Guid id)
    {
        return _members.FirstOrDefault(m => m.Id == id);
    }

    public void Add(Member member)
    {
        _members.Add(member);
    }
}
