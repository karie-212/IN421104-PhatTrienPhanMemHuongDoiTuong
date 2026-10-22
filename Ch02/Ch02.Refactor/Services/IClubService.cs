using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Services;

/// <summary>Nghiệp vụ quản lý CLB và thành viên.</summary>
public interface IClubService
{
    Club CreateClub(string name, string description, string presidentEmail);
    Member AddMember(Guid clubId, string memberName, string email);
}
