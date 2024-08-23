using Facebook.Server.Infrastructure.Logging;
using NLog;
using ILogger =  NLog.ILogger;

namespace Facebook.Server.Infrastructure.NLog;

public class LoggerService : ILoggerService
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public void LogDebug(string message) => _logger.Debug(message);

    public void LogDebug(Exception exception, string message) => _logger.Debug(exception, message);

    public void LogError(string message) => _logger.Error(message);

    public void LogError(Exception exception, string message) => _logger.Error(exception, message);

    public void LogInfo(string message) => _logger.Info(message);

    public void LogInfo(Exception exception, string message) => _logger.Info(exception, message);

    public void LogTrace(string message) => _logger.Trace(message);

    public void LogTrace(Exception exception, string message) => _logger.Trace(exception, message);

    public void LogWarn(string message) => _logger.Warn(message);

    public void LogWarn(Exception exception, string message) => _logger.Warn(exception, message);
}