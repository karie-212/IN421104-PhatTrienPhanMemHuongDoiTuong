namespace EduHub.Ch01.Domain;

/// <summary>
/// Phiếu đăng ký của Member cho Event (bảng nối M:N).
/// </summary>
public class Registration
{
    #region Constants for Status (Không dùng magic string)
    /// <summary>Trạng thái chờ xác nhận.</summary>
    public const string StatusPending = "Pending";

    /// <summary>Trạng thái đã xác nhận tham gia.</summary>
    public const string StatusConfirmed = "Confirmed";

    /// <summary>Trạng thái đã hủy đăng ký.</summary>
    public const string StatusCancelled = "Cancelled";
    #endregion

    /// <summary>Mã định danh duy nhất của lượt đăng ký.</summary>
    public Guid Id { get; private set; }

    /// <summary>Mã thành viên đăng ký (FK).</summary>
    public Guid MemberId { get; private set; }

    /// <summary>Mã sự kiện được đăng ký (FK).</summary>
    public Guid EventId { get; private set; }

    /// <summary>Thời điểm thực hiện đăng ký.</summary>
    public DateTime RegisteredAt { get; private set; }

    /// <summary>Trạng thái của lượt đăng ký.</summary>
    public string Status { get; private set; } = StatusPending;

    // Constructor PRIVATE tự sinh Id và RegisteredAt
    private Registration()
    {
        Id = Guid.NewGuid();
        RegisteredAt = DateTime.UtcNow;
    }

    private Registration(Guid memberId, Guid eventId) : this()
    {
        MemberId = memberId;
        EventId = eventId;
        Status = StatusPending;
    }

    /// <summary>
    /// Factory method khởi tạo lượt đăng ký mới với trạng thái mặc định là Pending.
    /// </summary>
    /// <param name="memberId">Mã thành viên.</param>
    /// <param name="eventId">Mã sự kiện.</param>
    /// <returns>Đối tượng Registration hợp lệ.</returns>
    public static Registration Create(Guid memberId, Guid eventId)
    {
        return new Registration(memberId, eventId);
    }

    /// <summary>
    /// Xác nhận lượt đăng ký (Chỉ hợp lệ khi đang ở trạng thái Pending).
    /// </summary>
    /// <exception cref="InvalidOperationException">Bắn ra khi trạng thái không phải Pending.</exception>
    public void Confirm()
    {
        if (Status != StatusPending)
        {
            throw new InvalidOperationException($"Không thể xác nhận lượt đăng ký đang ở trạng thái '{Status}'. Chỉ cho phép xác nhận khi đang '{StatusPending}'.");
        }

        Status = StatusConfirmed;
    }

    /// <summary>
    /// Hủy lượt đăng ký (Hợp lệ khi đang Pending hoặc Confirmed).
    /// </summary>
    /// <exception cref="InvalidOperationException">Bắn ra khi lượt đăng ký đã bị Cancelled từ trước.</exception>
    public void Cancel()
    {
        if (Status == StatusCancelled)
        {
            throw new InvalidOperationException("Lượt đăng ký này đã bị hủy từ trước đó.");
        }

        Status = StatusCancelled;
    }
}