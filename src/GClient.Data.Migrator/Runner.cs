using System;
using FluentMigrator.Runner;
using GClient.Data.Migrator.Migrations;
using GCommon.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace GClient.Data.Migrator
{
    internal static class Runner
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
                    .ScanIn(typeof(_001_InitialBuild).Assembly).For.Migrations())
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