using System;
using System.Diagnostics.CodeAnalysis;
using GCommon.Core;
using GCommon.Core.Enumerations;

namespace GCommon.Data.Entities
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ChangeLog
    {
        public int ChangeLogId { get; set; }
        public int ObjectId { get; set; }
        public DateTime ChangeDate { get; set; }
        internal int OperationId { get; private set; }
        public Operation Operation
        {
            get => Operation.FromValue(OperationId);
            set => OperationId = value.Value;
        }
        public int ConfigurationVersion { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public GalaxyObject GalaxyObject { get; set; }
    }
}