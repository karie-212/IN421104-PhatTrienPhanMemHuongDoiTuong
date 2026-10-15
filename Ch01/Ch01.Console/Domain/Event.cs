namespace EduHub.Ch01.Domain;

/// <summary>Sự kiện do câu lạc bộ tổ chức.</summary>
public class Event
{
    // TODO: Guid Id, Guid ClubId, string Title, DateTime Date, int Capacity, bool IsOnline (private set)

    // TODO: constructor PRIVATE + public static Event Create(Guid clubId, string title, DateTime date, int capacity, bool isOnline = false)
    //       → validate: title không trống, capacity > 0

    // TODO: public bool HasCapacity(int registeredCount) — true nếu registeredCount < Capacity
}
