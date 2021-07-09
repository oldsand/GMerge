using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using GalaxyMerge.Client.Application.Configurations;
using GalaxyMerge.Client.Application.Views;
using GalaxyMerge.Client.Core.Prism;
using GalaxyMerge.Client.Core.Prism.RegionBehaviors;
using GalaxyMerge.Client.Data.Abstractions;
using GalaxyMerge.Client.Data.Repositories;
using GalaxyMerge.Client.Dialogs;
using GalaxyMerge.Client.UI.Connection;
using GalaxyMerge.Data;
using GalaxyMerge.Data.Abstractions;
using NLog;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Application
{
    public partial class App
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        protected override Window CreateShell()
        {
            LoggerConfiguration.Apply();
            Logger.Debug("Log Configuration Applied - Creating new Application Shell");
            return Container.Resolve<Shell>();
        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            containerRegistry.RegisterSingleton<IRegionNavigationContentLoader, ScopedRegionNavigationContentLoader>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IGalaxyDataProviderFactory, GalaxyDataProviderFactory>();
            containerRegistry.Register<IResourceRepository, ResourceRepository>();
            containerRegistry.Register<ILogRepository, LogRepository>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<DialogModule>();
            moduleCatalog.AddModule<ConnectionModule>();
        }

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            regionBehaviors.AddIfMissing(RegionManagerAwareBehavior.BehaviorKey, typeof(RegionManagerAwareBehavior));
            regionBehaviors.AddIfMissing(DependentViewRegionBehavior.BehaviorKey, typeof(DependentViewRegionBehavior));
            base.ConfigureDefaultRegionBehaviors(regionBehaviors);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            RegisterUnhandledExceptions();
            base.OnStartup(e);
        }

        /// <summary>
        /// Attaches event handlers to prompt user when a unhandled exception occurs 
        /// </summary>
        private void RegisterUnhandledExceptions()
        {
            // Catch exceptions from all threads in the AppDomain.
            AppDomain.CurrentDomain.UnhandledException += (_, e) => 
                ShowException((Exception) e.ExceptionObject, nameof(AppDomain.CurrentDomain.UnhandledException), true);
            
            // Catch exceptions from each AppDomain that uses a task scheduler for async operations.
            TaskScheduler.UnobservedTaskException += (sender, args) =>
                ShowException(args.Exception, nameof(TaskScheduler.UnobservedTaskException), false);
            
            //Catch exceptions from the main UI dispatcher thread.
            Current.DispatcherUnhandledException += (sender, args) =>
            {
                if (Debugger.IsAttached) return;
                args.Handled = true;
                ShowException(args.Exception, nameof(Current.DispatcherUnhandledException), true);
            };
        }

        /// <summary>
        /// Shows user current unhandled exception 
        /// </summary>
        private void ShowException(Exception e, string unhandledType, bool promptUser)
        {
            var title = $"Unexpected Error Occurred: {unhandledType}";
            var message = $"The following exception occurred:\n\n{e.Message}";

            if (!promptUser) return;

            message += "\n\nNormally the app would die now. Should we let it die?";

            var dialogService = Container.Resolve<IDialogService>();
            if (dialogService != null)
            {
              dialogService.ShowError(title, message, e, result =>
              {
                  if (result.Result == ButtonResult.Yes)
                      Current.Shutdown();
              });  
              
              return;
            }
            
            const MessageBoxButton buttons = MessageBoxButton.YesNo;
            if (MessageBox.Show(message, title, buttons) == MessageBoxResult.Yes)
            {
                Current.Shutdown();      
            }
        }
    }
}