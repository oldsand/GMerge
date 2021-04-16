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
        private readonly IGalaxyFinder _galaxyFinder;
        private readonly IGalaxyRegistry _galaxyRegistry;
        private const string ChangeLogTableName = "gobject_change_log";
        private readonly List<SqlListener> _listeners;

        public GalaxyArchiveService(IGalaxyFinder galaxyFinder, IGalaxyRegistry galaxyRegistry)
        {
            _galaxyFinder = galaxyFinder;
            _galaxyRegistry = galaxyRegistry;
            _listeners = new List<SqlListener>();
        }

        public void Start()
        {
            RegisterGalaxies();

            Console.WriteLine("Starting Listeners");
            foreach (var listener in _listeners)
                listener.Start();
        }

        public void Stop()
        {
            var galaxies = _galaxyRegistry.GetUserGalaxies("admin");
            foreach (var galaxy in galaxies)
                _galaxyRegistry.UnregisterGalaxy(galaxy.Name, "admin");

            foreach (var listener in _listeners)
                listener.Stop();
        }

        private void RegisterGalaxies()
        {
            Console.WriteLine("Registering Galaxies");
            var galaxies = _galaxyFinder.FindAll();
            foreach (var galaxy in galaxies)
            {
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = Environment.MachineName,
                    InitialCatalog = galaxy,
                    IntegratedSecurity = true
                };

                Console.WriteLine($"Instantiating listener for galaxy '{galaxy}'");
                var listener = new SqlListener(builder.ConnectionString, galaxy, ChangeLogTableName);
                listener.TableChanged += OnChangeLogTableUpdated;
                _listeners.Add(listener);
                //TODO find out how to get local service account name here
                _galaxyRegistry.RegisterGalaxy(galaxy, "admin");
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
            return operationId == 0;
        }
    }
}