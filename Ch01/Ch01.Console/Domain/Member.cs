namespace EduHub.Ch01.Domain;

/// <summary>
/// Thành viên sinh viên của câu lạc bộ.
/// </summary>
public class Member
{
    /// <summary>Mã định danh duy nhất của thành viên.</summary>
    public Guid Id { get; private set; }

    /// <summary>Họ và tên thành viên.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Địa chỉ Email thành viên.</summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>Mã số sinh viên.</summary>
    public string StudentCode { get; private set; } = string.Empty;

    /// <summary>Thời điểm tham gia câu lạc bộ.</summary>
    public DateTime JoinedAt { get; private set; }

    /// <summary>Trạng thái hoạt động của thành viên.</summary>
    public MemberStatus Status { get; private set; }

    // Constructor PRIVATE tự sinh Id, JoinedAt và mặc định Status là Active
    private Member()
    {
        Id = Guid.NewGuid();
        JoinedAt = DateTime.UtcNow;
        Status = MemberStatus.Active;
    }

    private Member(string name, string email, string studentCode) : this()
    {
        Name = name;
        Email = email;
        StudentCode = studentCode;
    }

    /// <summary>
    /// Factory method khởi tạo thành viên với dữ liệu hợp lệ.
    /// </summary>
    public static Member Create(string name, string email, string studentCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên thành viên không được để trống.", nameof(name));

        if (string.IsNullOrWhiteSpace(studentCode))
            throw new ArgumentException("Mã số sinh viên không được để trống.", nameof(studentCode));

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Email không hợp lệ (phải chứa ký tự '@').", nameof(email));

        return new Member(name, email, studentCode);
    }

    /// <summary>
    /// Kiểm tra thành viên có đang ở trạng thái Active hay không.
    /// </summary>
    public bool IsActive() => Status == MemberStatus.Active;

    /// <summary>
    /// Chuyển trạng thái thành viên sang Inactive.
    /// </summary>
    public void Deactivate()
    {
        Status = MemberStatus.Inactive;
    }

    /// <summary>
    /// Hiển thị chuỗi định dạng tên và mã số sinh viên.
    /// </summary>
    public override string ToString()
    {
        return $"{Name} ({StudentCode}) - {Status}";
    }
}