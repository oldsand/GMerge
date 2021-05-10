using System;
using System.Collections.Generic;
using System.ServiceModel;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Common.Primitives;

namespace GalaxyMerge.Contracts
{
    [ServiceContract(Namespace = "http://www.gmerge.com/2014/Contracts")]
    public interface IArchiveService
    {
        [OperationContract]
        bool Connect(string galaxyName);
        
        [OperationContract]
        ArchiveObject GetObject(int objectId);

        [OperationContract]
        IEnumerable<ArchiveObject> GetObjects(string tagName);

        [OperationContract]
        IEnumerable<ArchiveEntry> GetEntries(int objectId);

        [OperationContract]
        IEnumerable<EventSetting> GetEventSettings();
        
        [OperationContract]
        IEnumerable<InclusionSetting> GetInclusionSettings();

        [OperationContract]
        void AddObject(int objectId);

        [OperationContract]
        void RemoveObject(int objectId);

        [OperationContract]
        void ArchiveObject(int objectId, bool force = false);

        [OperationContract]
        void UpdateEventSetting(IEnumerable<EventSetting> eventSettings);

        [OperationContract]
        void UpdateInclusionSetting(IEnumerable<InclusionSetting> inclusionSettings);
    }
}