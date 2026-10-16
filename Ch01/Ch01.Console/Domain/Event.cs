namespace EduHub.Ch01.Domain;

/// <summary>
/// Sự kiện do câu lạc bộ tổ chức.
/// </summary>
public class Event
{
    /// <summary>Mã định danh duy nhất của sự kiện.</summary>
    public Guid Id { get; private set; }

    /// <summary>Mã câu lạc bộ tổ chức sự kiện (FK).</summary>
    public Guid ClubId { get; private set; }

    /// <summary>Tên/Tiêu đề của sự kiện.</summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>Thời gian diễn ra sự kiện.</summary>
    public DateTime Date { get; private set; }

    /// <summary>Sức chứa tối đa (số lượng tham gia tối đa) của sự kiện.</summary>
    public int Capacity { get; private set; }

    /// <summary>Hình thức tổ chức (true nếu Online, false nếu Offline).</summary>
    public bool IsOnline { get; private set; }

    // Constructor PRIVATE tự sinh Id
    private Event()
    {
        Id = Guid.NewGuid();
    }

    private Event(Guid clubId, string title, DateTime date, int capacity, bool isOnline) : this()
    {
        ClubId = clubId;
        Title = title;
        Date = date;
        Capacity = capacity;
        IsOnline = isOnline;
    }

    /// <summary>
    /// Factory method khởi tạo sự kiện mới với dữ liệu hợp lệ.
    /// </summary>
    /// <param name="clubId">Mã câu lạc bộ tổ chức.</param>
    /// <param name="title">Tiêu đề sự kiện (không được để trống).</param>
    /// <param name="date">Thời gian diễn ra sự kiện.</param>
    /// <param name="capacity">Sức chứa tối đa (phải lớn hơn 0).</param>
    /// <param name="isOnline">Mặc định là false (Offline).</param>
    /// <returns>Đối tượng Event hợp lệ.</returns>
    /// <exception cref="ArgumentException">Bắn ra khi title bị trống hoặc capacity &lt;= 0.</exception>
    public static Event Create(Guid clubId, string title, DateTime date, int capacity, bool isOnline = false)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tiêu đề sự kiện không được để trống.", nameof(title));

        if (capacity <= 0)
            throw new ArgumentException("Sức chứa tối đa phải lớn hơn 0.", nameof(capacity));

        return new Event(clubId, title, date, capacity, isOnline);
    }

    /// <summary>
    /// Kiểm tra sự kiện còn sức chứa hay không dựa trên số lượng đã đăng ký.
    /// </summary>
    /// <param name="registeredCount">Số lượng đã đăng ký hiện tại.</param>
    /// <returns>True nếu số lượng đăng ký nhỏ hơn sức chứa tối đa.</returns>
    public bool HasCapacity(int registeredCount)
    {
        return registeredCount < Capacity;
    }
}