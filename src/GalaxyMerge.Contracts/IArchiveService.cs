using System.Collections.Generic;
using System.ServiceModel;
using GalaxyMerge.Archive.Entities;

namespace GalaxyMerge.Contracts
{
    [ServiceContract(Namespace = "http://www.gmerge.com/2014/Contracts")]
    public interface IArchiveService
    {
        [OperationContract]
        bool Connect(string galaxyName);
        
        [OperationContract]
        ArchiveObject GetArchiveObject(int objectId);

        [OperationContract]
        IEnumerable<ArchiveObject> GetArchiveObjects();

        [OperationContract]
        IEnumerable<ArchiveEntry> GetArchiveEntries();

        [OperationContract]
        GalaxyObjectData GetGalaxyObject(int objectId);
        
        [OperationContract]
        GalaxySymbolData GetGalaxySymbol(int objectId);

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