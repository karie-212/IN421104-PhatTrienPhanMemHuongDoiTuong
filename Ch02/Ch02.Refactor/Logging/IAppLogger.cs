namespace EduHub.Ch02.Logging;

/// <summary>Abstraction cho việc ghi log, để các service không phụ thuộc vào console hay file.</summary>
public interface IAppLogger
{
    void Info(string message);
}
