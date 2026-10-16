namespace EduHub.Ch01.Domain;

/// <summary>
/// Ghi nhận sự hiện diện thực tế của một Registration (quan hệ 1–1).
/// </summary>
public class CheckIn
{
    /// <summary>Mã định danh duy nhất của bản ghi điểm danh.</summary>
    public Guid Id { get; private set; }

    /// <summary>Mã lượt đăng ký tương ứng (FK).</summary>
    public Guid RegistrationId { get; private set; }

    /// <summary>Thời điểm thực hiện điểm danh.</summary>
    public DateTime CheckedInAt { get; private set; }

    /// <summary>Phương thức điểm danh ("Manual" hoặc "QrCode").</summary>
    public string Method { get; private set; } = string.Empty;

    // Constructor PRIVATE tự sinh Id và CheckedInAt
    private CheckIn()
    {
        Id = Guid.NewGuid();
        CheckedInAt = DateTime.UtcNow;
    }

    private CheckIn(Guid registrationId, string method) : this()
    {
        RegistrationId = registrationId;
        Method = method;
    }

    /// <summary>
    /// Factory method khởi tạo lượt điểm danh mới. Chỉ cho phép khi lượt đăng ký đã được xác nhận (Confirmed).
    /// </summary>
    /// <param name="registration">Đối tượng Registration tương ứng.</param>
    /// <param name="method">Phương thức điểm danh ("Manual" hoặc "QrCode").</param>
    /// <returns>Đối tượng CheckIn hợp lệ.</returns>
    /// <exception cref="ArgumentNullException">Bắn ra khi registration bị null.</exception>
    /// <exception cref="ArgumentException">Bắn ra khi phương thức điểm danh bị trống hoặc không hợp lệ.</exception>
    /// <exception cref="InvalidOperationException">Bắn ra khi lượt đăng ký chưa được Confirmed.</exception>
    public static CheckIn Create(Registration registration, string method)
    {
        ArgumentNullException.ThrowIfNull(registration);

        if (string.IsNullOrWhiteSpace(method))
            throw new ArgumentException("Phương thức điểm danh không được để trống.", nameof(method));

        if (registration.Status != Registration.StatusConfirmed)
        {
            throw new InvalidOperationException($"Chỉ cho phép điểm danh khi lượt đăng ký ở trạng thái '{Registration.StatusConfirmed}'. Trạng thái hiện tại: '{registration.Status}'.");
        }

        return new CheckIn(registration.Id, method);
    }

    /// <summary>
    /// Xuất chuỗi mô tả chi tiết thông tin điểm danh để in báo cáo.
    /// </summary>
    public string Summary()
    {
        return $"[CheckIn ID: {Id}] Registration: {RegistrationId} | Method: {Method} | Time: {CheckedInAt:yyyy-MM-dd HH:mm:ss}";
    }
}