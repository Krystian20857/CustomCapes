using System.IO;
using System.Reflection;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace CustomCapes.Common.Util {

    public static class LoggerUtil {

        public static void ConfigureLogger(string logsFolder) {
            var config = new LoggingConfiguration();
            var layout = new SimpleLayout("${longdate}|${level:uppercase=true}|${logger}|${message}  ${exception:format=ToString}");
            var logFile = new FileTarget("logfile")                                                 //logs will be saved at %AppData%/{AppName}/logs/
            {                                                                                             //logs are archived event day to %AppData%/{AppName}/logs/
                FileName = Path.Combine(logsFolder, "latest.log"),                                        //max archived logs files = 30
                KeepFileOpen = false,
                ArchiveFileName = Path.Combine(logsFolder, "log_${shortdate}.{##}.log"),
                ArchiveNumbering = ArchiveNumberingMode.Sequence,
                ArchiveEvery = FileArchivePeriod.Day,
                MaxArchiveFiles = 30,
                Layout = layout
            };

            var consoleLog = new ConsoleTarget()
            {
                AutoFlush = false,
                DetectConsoleAvailable = true,
                Layout = layout
            };

            var eventLog = new EventLogTarget()
            {
                Source = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location),
                Layout = layout
            };

            var coloredConsole = new ColoredConsoleTarget()
            {
                DetectConsoleAvailable = true,
                DetectOutputRedirected = true,
                AutoFlush = false,
                Layout = layout
            };

            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logFile);                   //all logs will be displayed to console and saved to file
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleLog);
            config.AddRule(LogLevel.Fatal, LogLevel.Fatal, eventLog);                  //only fatals and errors will be appear in eventlog
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, coloredConsole);

            LogManager.Configuration = config;
        }
        
    }

}