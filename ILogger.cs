namespace ProiectOOP;

public interface ILogger
{
    void Log(string message);
}

public class UserService
{
    private readonly ILogger _logger;

    public UserService(ILogger logger)
    {
        _logger = logger;
    }
}
public class ErrorLogger : ILogger
{
    private readonly string _logPath;
    private static readonly object _lock = new object();
    private readonly IFileSystem _fileSystem; // referinta fisier
    private readonly IConsole _console; // referinta consola

    public ErrorLogger(string logPath = "error_logs.txt")
    {
        _logPath = _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_logs.txt");
        _fileSystem = new FileSystemWrapper(); //initializeaza wrapper pt fisiere
        _console = new ConsoleWrapper(); // initializeaza wrapper pt consola.

    }

    public void Log(string message)
    {
        string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}\n";
        try
        {
            lock (_lock)
            {
                _fileSystem.AppendAllText(_logPath, logMessage); // foloseste wrapper pt scriere
            }
        }
        catch (Exception ex)
        {
            _console.WriteLine($"Failed to write to log file: {ex.Message}"); // foloseste wrapper-ul pt consola
        }
    }
}
