﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using ArchestrA.GRAccess;
using GServer.Archestra.Extensions;
using GCommon.Core;
using GServer.Archestra.Abstractions;
using GServer.Archestra.Exceptions;
using GServer.Archestra.Helpers;
using NLog;

namespace GServer.Archestra
{
    public class GalaxyRepository : IGalaxyRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable because this has to be in memory for operations to work (according to documentation)
        private readonly GRAccessAppClass _grAccessApp;

        public GalaxyRepository(string galaxyName)
        {
            if (string.IsNullOrEmpty(galaxyName))
                throw new ArgumentNullException(nameof(galaxyName), "Value can not be null");

            _grAccessApp = new GRAccessAppClass();
            Galaxy = _grAccessApp.QueryGalaxies(Environment.MachineName)[galaxyName];

            var result = _grAccessApp.CommandResult;
            if (!result.Successful || Galaxy == null)
                throw new GalaxyException(
                    $"Unable to load galaxy {galaxyName} on {Environment.MachineName}. Failed on {result.ID}. {result.CustomMessage}");
        }

        internal GalaxyRepository(GRAccessAppClass grAccessApp, IGalaxy galaxy)
        {
            _grAccessApp = grAccessApp ?? throw new ArgumentNullException(nameof(grAccessApp), "Value cannot be null");
            Galaxy = galaxy ?? throw new ArgumentNullException(nameof(galaxy), "Value cannot be null");
        }

        internal readonly IGalaxy Galaxy;
        public string Name => Galaxy.Name;
        public string Host => Environment.MachineName;
        public bool Connected { get; private set; }
        public string ConnectedUser { get; private set; }
        public string VersionString => Galaxy?.VersionString;
        public int? VersionNumber => Galaxy?.VersionNumber;
        public string CdiVersion => Galaxy?.CdiVersionString;

        public void Login(string userName)
        {
            Logger.Trace("Logging into galaxy {Galaxy} with user name {User}", Name, userName);

            Galaxy.SecureLogin(userName);

            Connected = true;
            ConnectedUser = userName;
        }

        public void Logout()
        {
            Logger.Trace("User {User} logging out of galaxy {Galaxy}", ConnectedUser, Name);

            Galaxy.Logout();
            Galaxy.CommandResult.Process();

            Connected = false;
            ConnectedUser = string.Empty;
        }

        public bool UserIsAuthorized(string userName)
        {
            Logger.Trace("Authorizing {User} against {Galaxy} current security settings", ConnectedUser, Name);

            Galaxy.SynchronizeClient();
            var security = Galaxy.GetReadOnlySecurity();
            Galaxy.CommandResult.Process();

            foreach (IGalaxyUser user in security.UsersAvailable)
                if (user.UserName == userName)
                    return true;

            return false;
        }

        public ArchestraObject GetObject(string tagName)
        {
            Galaxy.SynchronizeClient();

            var gObject = Galaxy.GetObjectByName(tagName);

            return gObject?.MapObject();
        }
        
        public ArchestraGraphic GetGraphic(string tagName)
        {
            Galaxy.SynchronizeClient();

            var symbol = Galaxy.GetSymbolByName(tagName);

            return symbol?.MapGraphic();
        }

        public void CreateObject(ArchestraObject source)
        {
            Galaxy.SynchronizeClient();

            GalaxyBuilder.On(this).For(source).Build();
        }

        public void CreateGraphic(ArchestraGraphic archestraGraphic)
        {
            Galaxy.SynchronizeClient();

            //todo user graphic builder
        }

        public void DeleteObject(string tagName, bool recursive)
        {
            Galaxy.SynchronizeClient();

            if (recursive)
            {
                Galaxy.DeepDelete(tagName);
                return;
            }

            var gObject = Galaxy.GetObjectByName(tagName);
            gObject?.Delete();
        }

        public void DeleteGraphic(string tagName)
        {
            Galaxy.SynchronizeClient();

            var gObject = Galaxy.GetSymbolByName(tagName);
            gObject?.DeleteInstance();
        }

        public void UpdateObject(ArchestraObject archestraObject)
        {
            Galaxy.SynchronizeClient();

            var repositoryObject = Galaxy.GetObjectByName(archestraObject.TagName);
            var original = repositoryObject.MapObject();

            /*try
            {
                repositoryObject.CheckOut();

                repositoryObject.SetUserDefinedAttributes(archestraObject);
                repositoryObject.SetFieldAttributes(archestraObject);
                repositoryObject.Save();

                repositoryObject.ConfigureAttributes(archestraObject);
                repositoryObject.Save();

                repositoryObject.ConfigureExtensions(archestraObject);
                repositoryObject.Save();

                repositoryObject.CheckIn($"Galaxy Merge Service Created Object '{archestraObject.TagName}'");
            }
            catch (Exception)
            {
                repositoryObject.CheckIn();
                repositoryObject.Delete();
                CreateObject(original);
                throw;
            }*/
        }

        public void UpdateGraphic(ArchestraGraphic archestraGraphic)
        {
            Galaxy.SynchronizeClient();

            //todo user graphic builder
        }
    }
}