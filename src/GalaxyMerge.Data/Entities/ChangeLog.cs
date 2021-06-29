// EF Core entity class. Only EF should be instantiating and setting properties.
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

using System;

namespace GalaxyMerge.Data.Entities
{
    public class ChangeLog
    {
        private ChangeLog()
        {
        }
        
        public int ChangeLogId { get; private set; }
        public int ObjectId { get; private set; }
        public DateTime ChangeDate { get; private set; }
        public short OperationId { get; private set; }
        public int ConfigurationVersion { get; private set; }
        public string Comment { get; private set; }
        public string UserName { get; private set; }
        public GObject GObject { get; private set; }
    }
}
