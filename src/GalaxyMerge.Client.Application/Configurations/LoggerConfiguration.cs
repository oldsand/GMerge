using System.Data;
using System.IO;
using GalaxyMerge.Core.Logging;
using GalaxyMerge.Core.Utilities;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace GalaxyMerge.Client.Application.Configurations
{
    public class LoggerConfiguration
    {
        public static void Apply()
        {
            ConfigurationItemFactory.Default.Targets.RegisterDefinition("MemoryEvent", typeof(MemoryEventTarget));

            var config = new LoggingConfiguration();

            var notificationTarget = new MemoryEventTarget("NotificationTarget");
            config.AddTarget(notificationTarget);
            config.LoggingRules.Add(new LoggingRule(LoggerName.NotificationLogger, LogLevel.Info, notificationTarget));

            var databaseTarget = new DatabaseTarget("DatabaseTarget")
            {
                DBProvider = "Microsoft.Data.Sqlite.SqliteConnection, Microsoft.Data.Sqlite",
                ConnectionString = $"Data Source={Path.Combine(ApplicationPath.Logging, "ClientLog.db")}",
                IsolationLevel = IsolationLevel.ReadCommitted,
                CommandText = @"insert into Log (MachineName, Logged, Level, Message, Logger, Properties, Callsite, Identity, Exception)
                        values (@MachineName, @Logged, @Level, @Message, @Logger, @Properties, @Callsite, @Identity, @Exception)",
                Parameters =
                {
                    new DatabaseParameterInfo("@MachineName", "${machinename}"),
                    new DatabaseParameterInfo("@Logged", "${date}"),
                    new DatabaseParameterInfo("@Level", "${level}"),
                    new DatabaseParameterInfo("@Message", "${message}"),
                    new DatabaseParameterInfo("@Logger", "${logger}"),
                    new DatabaseParameterInfo("@Properties", "${all-event-properties:separator=|}"),
                    new DatabaseParameterInfo("@Callsite", "${callsite}"),
                    new DatabaseParameterInfo("@Identity", "${windows-identity}"),
                    new DatabaseParameterInfo("@Exception", "${exception:tostring}")
                }
            };

            config.AddTarget(databaseTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, databaseTarget));

            LogManager.Configuration = config;
        }
    }
}