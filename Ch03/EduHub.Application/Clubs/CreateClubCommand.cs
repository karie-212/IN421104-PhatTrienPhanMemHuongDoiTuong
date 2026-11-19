using EduHub.Domain.Entities;
using EduHub.Domain.Interfaces;

namespace EduHub.Application.Clubs;

public record CreateClubCommand(string Name, string Description);

public class CreateClubCommandHandler
{
    private const int MaxNameLength = 50;
    private readonly IClubRepository _repository;

    public CreateClubCommandHandler(IClubRepository repository)
    {
        _repository = repository;
    }

    public async Task<Club> HandleAsync(CreateClubCommand command, CancellationToken ct = default)
    {
        // Validation 1: tên không rỗng / không toàn khoảng trắng
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Tên câu lạc bộ không được để trống.");

        // Validation 2: tên tối đa 50 ký tự
        if (command.Name.Length > MaxNameLength)
            throw new ArgumentException($"Tên câu lạc bộ tối đa {MaxNameLength} ký tự.");

        // Validation 3: tên không trùng
        string tenMoi = command.Name.Trim();
        var danhSach = await _repository.GetAllAsync(ct);
        bool daTonTai = danhSach.Any(c => c.Name.Equals(tenMoi, StringComparison.OrdinalIgnoreCase));
        if (daTonTai)
        {
            throw new InvalidOperationException("Câu lạc bộ '" + tenMoi + "' đã tồn tại.");
        }

        var club = Club.Create(command.Name.Trim(), command.Description ?? string.Empty);
        await _repository.AddAsync(club, ct);
        return club;
    }
}
