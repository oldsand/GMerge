using System;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using GalaxyMerge.Client.Data.MigrationRunner.Migrations;
using GalaxyMerge.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace GalaxyMerge.Client.Data.MigrationRunner
{
    internal class Migrator
    {
        private static void Main(string[] args)
        {
            var serviceProvider = CreateServices();
            using var scope = serviceProvider.CreateScope();
            UpdateDatabase(scope.ServiceProvider);
        }
        
        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString($"Data Source={ApplicationPath.ProgramData}\\app.db")
                    .ScanIn(typeof(InitialBuild).Assembly)
                        .For.Migrations()
                        .For.EmbeddedResources())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
        
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}