using System.Collections.Generic;
using GCommon.Primitives;
using GServer.Archestra.Entities;

namespace GServer.Archestra.IntegrationTests
{
    public static class Known
    {
        public static class Templates
        {
            public const string DrumConveyor = "$Drum_Conveyor";

            public static ArchestraObject ReactorSet => new()
            {
                TagName = "$ReactorSet",
                ConfigVersion = 94,
                Category = ObjectCategory.ApplicationObject,
                HierarchicalName = "$ReactorSet",
                BasedOnName = Template.UserDefined.Name,
                DerivedFromName = Template.UserDefined.Name,
                AreaName = "",
                HostName = "",
                ContainedName = "",
                ContainerName = "",
                Attributes = new List<ArchestraAttribute>
                {
                    new()
                    {
                        Name = "Auto",
                        DataType = DataType.Boolean,
                        Category = AttributeCategory.Writeable_UC_Lockable,
                        Locked = LockType.Unlocked,
                        Security = SecurityClassification.FreeAccess,
                        Value = false
                    },
                    new()
                    {
                        Name = "BatchNum",
                        DataType = DataType.Integer,
                        Category = AttributeCategory.Writeable_UC_Lockable,
                        Locked = LockType.Unlocked,
                        Security = SecurityClassification.FreeAccess,
                        Value = 0
                    },
                    new()
                    {
                        Name = "BatchPhase",
                        DataType = DataType.Integer,
                        Category = AttributeCategory.Writeable_UC_Lockable,
                        Locked = LockType.Unlocked,
                        Security = SecurityClassification.FreeAccess,
                        Value = 0
                    },
                    new()
                    {
                        Name = "ConcentratePercent",
                        DataType = DataType.Double,
                        Category = AttributeCategory.Writeable_UC_Lockable,
                        Locked = LockType.Unlocked,
                        Security = SecurityClassification.FreeAccess,
                        Value = 0.0
                    },
                    new()
                    {
                        Name = "Ref_Done",
                        DataType = DataType.Boolean,
                        Category = AttributeCategory.Writeable_UC_Lockable,
                        Locked = LockType.Unlocked,
                        Security = SecurityClassification.FreeAccess,
                        Value = false
                    },
                    new()
                    {
                        Name = "SimID",
                        DataType = DataType.String,
                        Category = AttributeCategory.Writeable_UC_Lockable,
                        Locked = LockType.Unlocked,
                        Security = SecurityClassification.FreeAccess,
                        Value = ""
                    },
                }
            };
        }

        public static class Instances
        {
            public const string R31 = nameof(R31);
            public const string DrumConveyor = "Drum_Conveyor";
        }
    }
}