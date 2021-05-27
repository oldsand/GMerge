using System.Collections.Generic;
using System.ServiceModel;
using GalaxyMerge.Contracts;

namespace GalaxyMerge.Client.Contracts
{
    [ServiceContract(Namespace = "http://www.gmerge.com/2014/Contracts")]
    public interface IArchiveService
    {
        [OperationContract]
        bool Connect(string galaxyName);
        
        [OperationContract]
        ArchiveObjectData GetArchiveObject(int objectId);

        [OperationContract]
        IEnumerable<ArchiveObjectData> GetArchiveObjects();

        [OperationContract]
        IEnumerable<ArchiveEntryData> GetArchiveEntries();

        [OperationContract]
        GalaxyObjectData GetGalaxyObject(int objectId);
        
        [OperationContract]
        GalaxySymbolData GetGalaxySymbol(int objectId);

        [OperationContract]
        IEnumerable<EventSettingData> GetEventSettings();
        
        [OperationContract]
        IEnumerable<InclusionSettingData> GetInclusionSettings();

        [OperationContract]
        void AddObject(int objectId);

        [OperationContract]
        void RemoveObject(int objectId);

        [OperationContract]
        void ArchiveObject(int objectId, bool force = false);

        [OperationContract]
        void UpdateEventSetting(IEnumerable<EventSettingData> eventSettings);

        [OperationContract]
        void UpdateInclusionSetting(IEnumerable<InclusionSettingData> inclusionSettings);
    }
}