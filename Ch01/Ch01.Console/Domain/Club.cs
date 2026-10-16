namespace EduHub.Ch01.Domain;

/// <summary>
/// Câu lạc bộ sinh viên.
/// </summary>
public class Club
{
    private readonly List<Member> _members = new();

    /// <summary>Mã định danh duy nhất của câu lạc bộ.</summary>
    public Guid Id { get; private set; }

    /// <summary>Tên câu lạc bộ.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Mô tả chi tiết về câu lạc bộ.</summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>Thời điểm thành lập câu lạc bộ.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Danh sách thành viên (chỉ đọc). Bên ngoài không thể .Add() trực tiếp.
    /// </summary>
    public IReadOnlyList<Member> Members => _members.AsReadOnly();

    // Constructor PRIVATE tự sinh Id và CreatedAt
    private Club()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    private Club(string name, string description) : this()
    {
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Factory method khởi tạo câu lạc bộ với thông tin hợp lệ.
    /// </summary>
    /// <param name="name">Tên câu lạc bộ (không được để trống).</param>
    /// <param name="description">Mô tả câu lạc bộ.</param>
    /// <returns>Đối tượng Club hợp lệ.</returns>
    /// <exception cref="ArgumentException">Bắn ra khi tên câu lạc bộ bị trống.</exception>
    public static Club Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên câu lạc bộ không được để trống.", nameof(name));

        return new Club(name, description);
    }

    /// <summary>
    /// Thêm thành viên mới vào câu lạc bộ (chặn null và thành viên trùng).
    /// </summary>
    /// <param name="member">Đối tượng Member cần thêm.</param>
    /// <exception cref="ArgumentNullException">Bắn ra khi member bị null.</exception>
    /// <exception cref="InvalidOperationException">Bắn ra khi thành viên đã tồn tại trong câu lạc bộ.</exception>
    public void AddMember(Member member)
    {
        ArgumentNullException.ThrowIfNull(member);

        if (_members.Any(m => m.Id == member.Id))
            throw new InvalidOperationException("Thành viên đã tồn tại trong câu lạc bộ.");

        _members.Add(member);
    }

    /// <summary>
    /// Lấy số lượng thành viên hiện tại của câu lạc bộ.
    /// </summary>
    public int MemberCount() => _members.Count;
}