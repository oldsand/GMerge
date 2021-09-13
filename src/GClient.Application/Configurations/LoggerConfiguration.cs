using System.Data;
using GCommon.Logging;
using GCommon.Utilities;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace GClient.Application.Configurations
{
    public class LoggerConfiguration
    {
        public static void Apply()
        {
            ConfigurationItemFactory.Default.Targets.RegisterDefinition("MemoryEvent", typeof(MemoryEventTarget));

            var config = new LoggingConfiguration();
            
            //var fileTarget = new FileTarget()

            var notificationTarget = new MemoryEventTarget(LoggerName.NotificationTarget);
            config.AddTarget(notificationTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, notificationTarget));

            var databaseTarget = new DatabaseTarget(LoggerName.DatabaseTarget)
            {
                DBProvider = "Microsoft.Data.Sqlite.SqliteConnection, Microsoft.Data.Sqlite",
                ConnectionString = $"Data Source={ApplicationPath.ProgramData}\\app.db",
                IsolationLevel = IsolationLevel.ReadCommitted,
                CommandText =
                    @"insert into Log (Logged, LevelId, Level, Message, Logger, Properties, Callsite, FileName, LineNumber, Stacktrace, MachineName, Identity, Exception)
                        values (@Logged, @LevelId, @Level, @Message, @Logger, @Properties, @Callsite, @FileName, @LineNumber, @Stacktrace, @MachineName, @Identity, @Exception)",
                Parameters =
                {
                    new DatabaseParameterInfo("@Logged", "${date}"),
                    new DatabaseParameterInfo("@LevelId", "${level:format=Ordinal}"),
                    new DatabaseParameterInfo("@Level", "${level:uppercase=true}"),
                    new DatabaseParameterInfo("@Message", "${message}"),
                    new DatabaseParameterInfo("@Logger", "${logger}"),
                    new DatabaseParameterInfo("@Properties", "${all-event-properties:separator=|}"),
                    new DatabaseParameterInfo("@Callsite", "${callsite}"),
                    new DatabaseParameterInfo("@FileName", "${callsite-filename}"),
                    new DatabaseParameterInfo("@LineNumber", "${callsite-linenumber}"),
                    new DatabaseParameterInfo("@Stacktrace", "${stacktrace}"),
                    new DatabaseParameterInfo("@MachineName", "${machinename}"),
                    new DatabaseParameterInfo("@Identity", "${windows-identity}"),
                    new DatabaseParameterInfo("@Exception", "${exception:tostring}")
                }
            };

            config.AddTarget(databaseTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, databaseTarget));

            LogManager.ThrowExceptions = true;
            LogManager.Configuration = config;
        }
    }
}