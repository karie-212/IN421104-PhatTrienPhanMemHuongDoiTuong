using EduHub.Ch02.Domain;
using EduHub.Ch02.Logging;

namespace EduHub.Ch02.Patterns.Observers;

/// <summary>Ghi log mỗi khi có sự kiện mới.</summary>
public class LogNotifier : IEventObserver
{
    private readonly IAppLogger _logger;

    public LogNotifier(IAppLogger logger)
    {
        _logger = logger;
    }

    public void OnEventCreated(Event ev, Club club)
    {
        _logger.Info("Đã tạo sự kiện '" + ev.Title + "' (" + club.Name + ")");
    }
}
