namespace EduHub.Ch02.Logging;

/// <summary>Ghi log nối tiếp vào file, mỗi dòng kèm thời gian.</summary>
public class FileLogger : IAppLogger
{
    private readonly string _filePath;

    public FileLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Info(string message)
    {
        File.AppendAllText(_filePath, $"[{DateTime.Now}] {message}\n");
    }
}
