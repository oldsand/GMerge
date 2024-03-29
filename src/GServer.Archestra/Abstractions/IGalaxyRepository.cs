﻿using System.Collections.Generic;
using GCommon.Core;

namespace GServer.Archestra.Abstractions
{
    public interface IGalaxyRepository
    {
        string Name { get; }
        string Host { get; }
        bool Connected { get; }
        string ConnectedUser { get; }
        string VersionString { get; }
        int? VersionNumber { get; }
        string CdiVersion { get; }
        void Login(string userName);
        void Logout();
        bool UserIsAuthorized(string userName);
        ArchestraObject GetObject(string tagName);
        ArchestraGraphic GetGraphic(string tagName);
        void CreateObject(ArchestraObject source);
        void CreateGraphic(ArchestraGraphic archestraGraphic);
        void DeleteObject(string tagName, bool recursive);
        void DeleteGraphic(string tagName);
        void UpdateObject(ArchestraObject archestraObject);
        void UpdateGraphic(ArchestraGraphic archestraGraphic);
    }
}