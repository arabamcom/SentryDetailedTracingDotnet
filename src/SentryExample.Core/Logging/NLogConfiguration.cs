using NLog;
using NLog.Config;
using NLog.Targets;

namespace SentryExample.Core.Logging
{
    public static class NLogConfiguration
    {
        public static LoggingConfiguration? ConfigureTarget()
        {
            var configuration = LogManager.Configuration;
            var fileTarget = new FileTarget
            {
                FileName = "${basedir}\\Logger\\internal_logs\\logFile.txt",
                Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}",
            };
            configuration.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Info, fileTarget, "*", false);
            configuration.AddSentry(o =>
            {
                o.Dsn = "https://d700368ca9694755822d1c91431470e4@apm.arabam.com/12";
                o.Layout = "${message}";
                o.BreadcrumbLayout = "${logger}: ${message}";
                o.MinimumBreadcrumbLevel = NLog.LogLevel.Info;
                o.MinimumEventLevel = NLog.LogLevel.Warn;
                o.AddTag("logger", "${logger}");
            });
            return configuration;
        }
    }
}
