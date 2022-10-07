using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

namespace Play.Logging;

public class SerilogSettings
{
    #region Instance Values

    private readonly string _OutputFormat = "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";

    private string _LogFileName = string.Empty;

    private string _LogLevel = LogEventLevel.Debug.ToString();

    private string _RollingInterval = Serilog.RollingInterval.Day.ToString();

    public string LogFileName
    {
        get => _LogFileName == string.Empty ? "Log.txt" : _LogFileName;
        set => _LogFileName = $"{value.Replace(".txt", "")}.txt";
    }

    public string LogLevel
    {
        get => _LogLevel;
        set
        {
            if (!Enum.TryParse<LogEventLevel>(value, true, out LogEventLevel result))
                return;
            else
                _LogLevel = result.ToString();
        }
    }

    public string RollingInterval
    {
        get => _RollingInterval;
        set
        {
            if (!Enum.TryParse<RollingInterval>(value, true, out RollingInterval result))
                return;
            else
                _RollingInterval = result.ToString();
        }
    }

    #endregion

    #region Instance Members

    public LoggerConfiguration AsLoggerConfiguration(AppDomain baseDirectory, IHostEnvironment webHostEnvironment)
    {
        LogEventLevel logEventLevel = Enum.Parse<LogEventLevel>(LogLevel);
        RollingInterval rollingInterval = Enum.Parse<RollingInterval>(RollingInterval);
        string filePath = Path.Combine(baseDirectory.BaseDirectory, LogFileName);

        return new LoggerConfiguration().WriteTo.Console(outputTemplate: _OutputFormat, restrictedToMinimumLevel: logEventLevel)
            .WriteTo.File(Path.Combine(filePath, LogFileName), logEventLevel, _OutputFormat, rollingInterval: rollingInterval)
            .Enrich.FromLogContext();
    }

    #endregion
}