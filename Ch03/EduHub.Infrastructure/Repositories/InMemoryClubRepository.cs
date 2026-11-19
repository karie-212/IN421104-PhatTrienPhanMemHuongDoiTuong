using EduHub.Domain.Entities;
using EduHub.Domain.Interfaces;

namespace EduHub.Infrastructure.Repositories;

/// <summary>
/// Lưu trữ tạm trong memory — dùng cho Ch03 khi chưa có database.
/// Ch04 sẽ thay bằng EfClubRepository dùng PostgreSQL.
/// </summary>
public class InMemoryClubRepository : IClubRepository
{
    // static: repository đăng ký Scoped (mới mỗi request) nhưng dữ liệu phải sống suốt vòng đời app.
    private static readonly List<Club> _clubs = new();
    private static readonly object _lock = new();

    public Task<IEnumerable<Club>> GetAllAsync(CancellationToken ct = default)
    {
        lock (_lock)
            // Trả về bản sao để bên ngoài không sửa trực tiếp danh sách gốc
            return Task.FromResult<IEnumerable<Club>>(_clubs.ToList());
    }

    public Task<Club?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        lock (_lock)
            // Không tìm thấy thì trả null, KHÔNG throw
            return Task.FromResult(_clubs.FirstOrDefault(c => c.Id == id));
    }

    public Task AddAsync(Club club, CancellationToken ct = default)
    {
        lock (_lock) _clubs.Add(club);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Club club, CancellationToken ct = default)
    {
        lock (_lock)
        {
            var index = _clubs.FindIndex(c => c.Id == club.Id);
            if (index < 0)
                throw new KeyNotFoundException($"Không tìm thấy club {club.Id}");
            _clubs[index] = club;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        lock (_lock)
        {
            var club = _clubs.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Không tìm thấy club {id}");
            _clubs.Remove(club);
        }
        return Task.CompletedTask;
    }
}
