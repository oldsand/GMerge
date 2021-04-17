using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Contracts.Data;
using GalaxyMerge.Core.Extensions;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Data.Repositories;

namespace GalaxyMerge.Services
{
    public class GalaxyArchiveService
    {
        
        private readonly IGalaxyRegistry _galaxyRegistry;
        private readonly List<SqlListener> _listeners;
        private const string ChangeLogTableName = "gobject_change_log";
        private const int CheckInOperationId = 0;

        public GalaxyArchiveService(IGalaxyRegistry galaxyRegistry)
        {
            _galaxyRegistry = galaxyRegistry;
            _listeners = new List<SqlListener>();
        }

        public void Start()
        {
            InitializeListeners();

            Console.WriteLine("Starting Listeners");
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
            Console.WriteLine("Initializing Listeners");
            
            var galaxies = _galaxyRegistry.GetAllGalaxies();
            
            foreach (var galaxy in galaxies)
            {
                var connectionString = ConnectionStringBuilder.BuildGalaxyConnection(galaxy.Name);

                Console.WriteLine($"Instantiating listener for galaxy '{galaxy}'");
                
                var listener = new SqlListener(connectionString, galaxy.Name, ChangeLogTableName);
                listener.TableChanged += OnChangeLogTableUpdated;
                _listeners.Add(listener);
            }
        }

        private void OnChangeLogTableUpdated(object sender, SqlListener.TableChangedEventArgs e)
        {
            if (!(sender is SqlListener listener)) return;
            if (!IsCheckInOperation(e.Data, listener.ConnectionString)) return;

            var gObject = GetObject(e.Data, listener.ConnectionString);
            var galaxy = _galaxyRegistry.GetGalaxy(listener.DatabaseName, "admin");
            var xml = GetXmlData(gObject, galaxy);
            ArchiveData(gObject, xml, listener);
        }

        private static void ArchiveData(GObject gObject, XNode xml, SqlListener listener)
        {
            var archive = new ArchiveRepository(listener.DatabaseName);
            var entry = new ArchiveEntry(gObject.ObjectId, gObject.TagName, gObject.ConfigVersion,
                gObject.Template.TagName, xml.ToByteArray());
            archive.AddEntry(entry);
            archive.Save();
        }

        private static XElement GetXmlData(GObject gObject, IGalaxyRepository galaxy)
        {
            if (gObject.Template.TagName == "$Symbol")
            {
                var galaxySymbol = (GalaxySymbol) galaxy.GetSymbol(gObject.TagName);
                return galaxySymbol.ToXml();
            }

            var galaxyObject = (GalaxyObject) galaxy.GetObject(gObject.TagName);
            return galaxyObject.ToXml();
        }

        private static GObject GetObject(XContainer data, string connectionString)
        {
            var repository = new ObjectRepository(connectionString);
            var id = Convert.ToInt32(data.Descendants("gobject_id").Select(e => e.Value).SingleOrDefault());
            return repository.FindByIdIncludeTemplate(id);
        }

        private static bool IsCheckInOperation(XContainer data, string connectionString)
        {
            var operationId = Convert.ToInt32(data.Descendants("operation_id").Select(e => e.Value).SingleOrDefault());
            return operationId == CheckInOperationId;
        }
    }
}