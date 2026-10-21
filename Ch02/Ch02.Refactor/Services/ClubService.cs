using EduHub.Ch02.Domain;
using EduHub.Ch02.Logging;
using EduHub.Ch02.Notifications;
using EduHub.Ch02.Repositories;

namespace EduHub.Ch02.Services;

/// <summary>
/// Tách từ God class cũ: chỉ điều phối nghiệp vụ CLB và thành viên.
/// Mọi phụ thuộc là interface nhận qua constructor, không new class cụ thể nào (DIP).
/// Không biết NotificationServiceFactory, chỉ dùng INotificationService đã được tạo sẵn.
/// </summary>
public class ClubService : IClubService
{
    private readonly IClubRepository _clubs;
    private readonly IMemberRepository _members;
    private readonly INotificationService _notifier;
    private readonly IAppLogger _logger;

    public ClubService(
        IClubRepository clubs,
        IMemberRepository members,
        INotificationService notifier,
        IAppLogger logger)
    {
        _clubs = clubs;
        _members = members;
        _notifier = notifier;
        _logger = logger;
    }

    public Club CreateClub(string name, string description, string presidentEmail)
    {
        // Validate nằm ở Domain.
        var club = Club.Create(name, description);

        _clubs.Add(club);
        _logger.Info("Đã tạo CLB: " + club.Name);
        _notifier.Send(presidentEmail, "CLB '" + club.Name + "' đã được tạo");

        return club;
    }

    public Member AddMember(Guid clubId, string memberName, string email)
    {
        var club = _clubs.GetById(clubId)
            ?? throw new InvalidOperationException("Không tìm thấy CLB");

        if (!club.HasRoom(_members.GetByClub(clubId).Count))
            throw new InvalidOperationException("CLB đã đủ " + Club.MaxMembers + " thành viên");

        var member = new Member { Id = Guid.NewGuid(), Name = memberName, Email = email, ClubId = clubId };

        _members.Add(member);
        _logger.Info("Thành viên " + memberName + " đã tham gia " + club.Name);
        _notifier.Send(email, "Chào mừng " + memberName + " đến với " + club.Name);

        return member;
    }
}
