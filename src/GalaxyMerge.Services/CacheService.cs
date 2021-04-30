using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Xml.Linq;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Data.Repositories;

namespace GalaxyMerge.Services
{
    public class GalaxyCachingService
    {
        
        private readonly IGalaxyRegistry _galaxyRegistry;
        private readonly List<SqlListener> _listeners;
        private const string ChangeLogTableName = "gobject_change_log";
        private const int CheckInOperationId = 0;
        private readonly string _serviceUserName;

        public GalaxyCachingService(IGalaxyRegistry galaxyRegistry)
        {
            _galaxyRegistry = galaxyRegistry;
            _listeners = new List<SqlListener>();
            _serviceUserName = WindowsIdentity.GetCurrent().Name;
        }

        public void Start()
        {
            InitializeListeners();
            //Is this where I should validate that galaxy dbs all have the 
            
            foreach (var listener in _listeners)
                listener.Start();
        }

        public void Stop()
        {
            foreach (var listener in _listeners)
                listener.Stop();
        }

        private void InitializeListeners()
        {
            var galaxies = _galaxyRegistry.GetByUser(_serviceUserName);
            foreach (var galaxy in galaxies)
            {
                var connectionString = ConnectionStringBuilder.BuildGalaxyConnection(galaxy.Name);
                var listener = new SqlListener(connectionString, galaxy.Name, ChangeLogTableName);
                listener.TableChanged += OnChangeLogTableUpdated;
                _listeners.Add(listener);
            }
        }

        private void OnChangeLogTableUpdated(object sender, SqlListener.TableChangedEventArgs e)
        {
            if (!(sender is SqlListener listener)) return;
            if (!IsCheckInOperation(e.Data)) return;
            
            var galaxyRepo = _galaxyRegistry.GetGalaxy(listener.DatabaseName, _serviceUserName);
            //var repo = RepositoryFactory.Instance<GObject, IObjectRepository>(listener.ConnectionString); Is this really better?
            var objectRepo = new ObjectRepository(listener.ConnectionString);
            var archiveRepo = new ArchiveRepository(ConnectionStringBuilder.BuildArchiveConnection(listener.DatabaseName));
            var archiver = new GalaxyArchiver(galaxyRepo, objectRepo, archiveRepo);
            
            var objectId = ExtractObjectId(e.Data);
            archiver.Archive(objectId);
        }

        private static int ExtractObjectId(XContainer data)
        {
            return Convert.ToInt32(data.Descendants("gobject_id").Select(x => x.Value).SingleOrDefault());
        }
        
        private static bool IsCheckInOperation(XContainer data)
        {
            var operationId = Convert.ToInt32(data.Descendants("operation_id").Select(e => e.Value).SingleOrDefault());
            return operationId == CheckInOperationId;
        }
    }
}