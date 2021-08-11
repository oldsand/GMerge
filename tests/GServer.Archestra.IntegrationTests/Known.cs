using System.Collections.Generic;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;

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
                    new("Auto", DataType.Boolean)
                    {
                        Category = AttributeCategory.Writeable_UC_Lockable,
                        Locked = LockType.Unlocked,
                        Security = SecurityClassification.FreeAccess,
                        Value = false
                    },
                    new("BatchNum", DataType.Integer)
                    {
                        Category = AttributeCategory.Writeable_UC_Lockable,
                        Locked = LockType.Unlocked,
                        Security = SecurityClassification.FreeAccess,
                        Value = 0
                    },
                    new("BatchPhase", DataType.Integer)
                    {
                        Category = AttributeCategory.Writeable_UC_Lockable,
                        Locked = LockType.Unlocked,
                        Security = SecurityClassification.FreeAccess,
                        Value = 0
                    },
                    new("ConcentratePercent", DataType.Double)
                    {
                        Category = AttributeCategory.Writeable_UC_Lockable,
                        Locked = LockType.Unlocked,
                        Security = SecurityClassification.FreeAccess,
                        Value = 0.0
                    },
                    new("Ref_Done", DataType.Boolean)
                    {
                        Category = AttributeCategory.Writeable_UC_Lockable,
                        Locked = LockType.Unlocked,
                        Security = SecurityClassification.FreeAccess,
                        Value = false
                    },
                    new("SimID", DataType.String)
                    {
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