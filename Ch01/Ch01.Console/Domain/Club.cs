namespace EduHub.Ch01.Domain;

/// <summary>Câu lạc bộ sinh viên.</summary>
public class Club
{
    // TODO: Guid Id, string Name, string Description, DateTime CreatedAt  (private set)
    // TODO: danh sách thành viên: field private List<Member> + property IReadOnlyList<Member> Members
    //       (bên ngoài không được .Add() trực tiếp — phải đi qua AddMember)

    // TODO: constructor PRIVATE + public static Club Create(string name, string description)
    // TODO: public void AddMember(Member member) — chặn null và thành viên trùng
    // TODO: public int MemberCount()
}
