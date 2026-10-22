using EduHub.Ch02.Domain;
using EduHub.Ch02.Logging;

namespace EduHub.Ch02.Repositories;

/// <summary>
/// Decorator: implement cùng interface IClubRepository và bọc một IClubRepository gốc (inner).
/// Mỗi lời gọi được ghi log trước rồi chuyển tiếp sang inner, nên thêm được việc ghi log
/// mà không phải sửa InMemoryClubRepository; nơi dùng vẫn chỉ thấy IClubRepository.
/// </summary>
public class LoggingClubRepository : IClubRepository
{
    private readonly IClubRepository _inner;
    private readonly IAppLogger _logger;

    public LoggingClubRepository(IClubRepository inner, IAppLogger logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public IReadOnlyList<Club> GetAll()
    {
        _logger.Info("[repo] Lấy danh sách CLB");
        return _inner.GetAll();
    }

    public Club? GetById(Guid id)
    {
        _logger.Info("[repo] Tìm CLB theo Id " + id);
        return _inner.GetById(id);
    }

    public void Add(Club club)
    {
        _logger.Info("[repo] Thêm CLB '" + club.Name + "'");
        _inner.Add(club);
    }
}
