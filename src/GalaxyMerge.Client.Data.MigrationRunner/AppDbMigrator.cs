using System;
using FluentMigrator.Runner;
using GalaxyMerge.Client.Data.MigrationRunner.Migrations;
using GalaxyMerge.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace GalaxyMerge.Client.Data.MigrationRunner
{
    internal class AppDbMigrator
    {
        private static void Main(string[] args)
        {
            var serviceProvider = CreateServices();
            using var scope = serviceProvider.CreateScope();
            UpdateDatabase(scope.ServiceProvider);
        }
        
        /// <summary>
        /// Configure the dependency injection services
        /// </summary>
        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString($"Data Source={ApplicationPath.ProgramData}\\app.db")
                    .ScanIn(typeof(UpdateResourceTypeConversion).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
        
        /// <summary>
        /// Update the database
        /// </summary>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}