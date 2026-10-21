namespace EduHub.Ch02.Domain;

/// <summary>Câu lạc bộ. Quy tắc nghiệp vụ nằm ở đây, không nằm trong service.</summary>
public class Club
{
    public const int MaxMembers = 50;
    public const int MaxNameLength = 100;

    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    // Factory kiểm tra luật đặt tên trước khi tạo CLB.
    public static Club Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên CLB không được để trống");

        if (name.Length > MaxNameLength)
            throw new ArgumentException("Tên CLB không được vượt quá " + MaxNameLength + " ký tự");

        return new Club
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description
        };
    }

    // Còn chỗ khi số thành viên hiện tại nhỏ hơn MaxMembers.
    public bool HasRoom(int currentMemberCount)
    {
        return currentMemberCount < MaxMembers;
    }
}
