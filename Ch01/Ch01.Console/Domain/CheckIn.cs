namespace EduHub.Ch01.Domain;

/// <summary>Ghi nhận sự hiện diện thực tế của một Registration (quan hệ 1–1).</summary>
public class CheckIn
{
    // TODO: Guid Id, Guid RegistrationId, DateTime CheckedInAt, string Method ("Manual" hoặc "QrCode") (private set)

    // TODO: constructor PRIVATE + public static CheckIn Create(Registration registration, string method)
    //       → chỉ cho phép khi registration.Status == Confirmed

    // TODO: public string Summary() — chuỗi mô tả để in báo cáo
}
