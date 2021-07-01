using System;
using System.Windows.Controls;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Core.Utilities;
using GalaxyMerge.Data.Entities;
using Prism.Regions;

namespace GalaxyMerge.Client.UI.Connection.ViewModels
{
    public class GalaxyObjectViewModel : NavigationViewModelBase
    {

        private GObject _galaxyObject;

        public GObject GalaxyObject
        {
            get => _galaxyObject;
            set => SetProperty(ref _galaxyObject, value);
        }
        
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var gObject = navigationContext.Parameters.GetValue<GObject>("object");
            GalaxyObject = gObject ?? throw new ArgumentNullException(nameof(gObject), 
                               @"Value for gObject cannot be null");

            Title = gObject.TagName;
            Icon = ResourceFinder.Find<ControlTemplate>("Icon.Galaxy.ApplicationObject");
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var gObject = navigationContext.Parameters.GetValue<GObject>("object");
            return gObject.ObjectId == GalaxyObject.ObjectId;
        }
    }
}