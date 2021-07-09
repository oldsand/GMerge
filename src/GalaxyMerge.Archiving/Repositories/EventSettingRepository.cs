using System;
using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Archiving.Abstractions;
using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archiving.Repositories
{
    internal class EventSettingRepository : IEventSettingsRepository
    {
        private readonly ArchiveContext _context;

        public EventSettingRepository(ArchiveContext context)
        {
            _context = context;
        }
        
        public bool IsTrigger(Operation operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation), "operation can not be null");
            return _context.EventSettings.Single(x => x.Operation == operation).IsArchiveEvent;
        }

        public EventSetting Get(Operation operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation), "operation can not be null");
            return _context.EventSettings.Single(x => x.Operation == operation);
        }

        public IEnumerable<EventSetting> GetAll()
        {
            return _context.EventSettings.ToList();
        }

        public IEnumerable<EventSetting> GetArchiveEvents()
        {
            return _context.EventSettings.Where(x => x.IsArchiveEvent);
        }

        public void Configure(Operation operation, bool isArchiveEvent = true)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation), "operation can not be null");
            
            var target = _context.EventSettings.Single(x => x.Operation == operation);
            target.IsArchiveEvent = isArchiveEvent;
        }
        
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}