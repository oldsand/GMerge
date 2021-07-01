using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using GalaxyMerge.Contracts;

namespace GalaxyMerge.Client.Proxies
{
    public class ArchiveClient : ClientBase<IArchiveService>, IArchiveService
    {
        public ArchiveClient(Binding binding, EndpointAddress address)
            : base(binding, address)
        {
        }
        
        public bool Connect(string galaxyName)
        {
            return Channel.Connect(galaxyName);
        }

        public ArchiveObjectData GetArchiveObject(int objectId)
        {
            return Channel.GetArchiveObject(objectId);
        }

        public IEnumerable<ArchiveObjectData> GetArchiveObjects()
        {
            return Channel.GetArchiveObjects();
        }

        public IEnumerable<ArchiveEntryData> GetArchiveEntries()
        {
            return Channel.GetArchiveEntries();
        }

        public GalaxyObjectData GetGalaxyObject(int objectId)
        {
            return Channel.GetGalaxyObject(objectId);
        }

        public GalaxySymbolData GetGalaxySymbol(int objectId)
        {
            return Channel.GetGalaxySymbol(objectId);
        }

        public IEnumerable<EventSettingData> GetEventSettings()
        {
            return Channel.GetEventSettings();
        }

        public IEnumerable<InclusionSettingData> GetInclusionSettings()
        {
            return Channel.GetInclusionSettings();
        }

        public void AddObject(int objectId)
        {
            Channel.AddObject(objectId);
        }

        public void RemoveObject(int objectId)
        {
            Channel.RemoveObject(objectId);
        }

        public void ArchiveObject(int objectId, bool force = false)
        {
            Channel.ArchiveObject(objectId, force);
        }

        public void UpdateEventSetting(IEnumerable<EventSettingData> eventSettings)
        {
            Channel.UpdateEventSetting(eventSettings);
        }

        public void UpdateInclusionSetting(IEnumerable<InclusionSettingData> inclusionSettings)
        {
            Channel.UpdateInclusionSetting(inclusionSettings);
        }
    }
}