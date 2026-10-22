namespace EduHub.Ch02.Logging;

/// <summary>Ghi log ra console.</summary>
public class ConsoleLogger : IAppLogger
{
    public void Info(string message)
    {
        Console.WriteLine("  [LOG] " + message);
    }
}
