using System.Data;
using System.IO;
using GCommon.Core.Utilities;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace GalaxyMerge.Host.Configurations
{
    public static class LoggerConfiguration
    {
        public static void Apply()
        {
            var config = new LoggingConfiguration();

            var databaseTarget = new DatabaseTarget("DatabaseTarget")
            {
                DBProvider = "Microsoft.Data.Sqlite.SqliteConnection, Microsoft.Data.Sqlite",
                ConnectionString = $"Data Source={Path.Combine(ApplicationPath.Logging, "ServiceLog.db")}",
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

            var eventLogTarget = new EventLogTarget("EventLogTarget")
            {
                MachineName = ".",
                Log = "Application",
                Source = "gmerge",
                Layout = "${logger}: ${message}"
            };
                
            config.AddTarget(eventLogTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, eventLogTarget));

            LogManager.Configuration = config;
        }
    }
}