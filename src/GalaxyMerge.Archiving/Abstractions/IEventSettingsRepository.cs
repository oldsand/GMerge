using System;
using System.Collections.Generic;
using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archiving.Abstractions
{
    public interface IEventSettingsRepository : IDisposable
    {
        bool IsTrigger(Operation operation);
        EventSetting Get(Operation operation);
        IEnumerable<EventSetting> GetAll();
        IEnumerable<EventSetting> GetArchiveEvents();
        void Configure(Operation operation, bool isArchiveEvent);
    }
}