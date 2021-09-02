using System.Collections.Generic;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;

namespace GServer.Archestra.IntegrationTests.Base
{
    public static class Known
    {
        public static class Templates
        {
            public static ArchestraObject DrumConveyor => new()
            {
                TagName = "$Drum_Conveyor",
                ConfigVersion = 86,
                Category = ObjectCategory.ApplicationObject,
                HierarchicalName = "$Drum_Conveyor",
                BasedOnName = Template.UserDefined.Name,
                DerivedFromName = Template.UserDefined.Name,
                Attributes = new List<ArchestraAttribute>
                {
                    new("Auto", DataType.Boolean),
                    new("Cycle", DataType.Integer),
                    new("DrumCount", DataType.Integer, security: SecurityClassification.FreeAccess),
                    new("Ejector", DataType.Boolean),
                    new("HorizontalMovement", DataType.Double),
                    new("PanelPCPower", DataType.Boolean, security: SecurityClassification.FreeAccess),
                    new("Ref_Done", DataType.Boolean),
                    new("ScreenWidth", DataType.Integer, security: SecurityClassification.FreeAccess, value: 1280),
                    new("SIMID", DataType.String, value: "ConveyorSim"),
                    new("Speed", DataType.Double, security: SecurityClassification.FreeAccess),
                    new("VerticalMovement", DataType.Double),
                }
            };

            public static ArchestraObject ReactorSet => new()
            {
                TagName = "$ReactorSet",
                ConfigVersion = 94,
                Category = ObjectCategory.ApplicationObject,
                HierarchicalName = "$ReactorSet",
                BasedOnName = Template.UserDefined.Name,
                DerivedFromName = Template.UserDefined.Name,
                Attributes = new List<ArchestraAttribute>
                {
                    new("Auto", DataType.Boolean, security: SecurityClassification.Tune),
                    new("BatchNum", DataType.Integer),
                    new("BatchPhase", DataType.Integer),
                    new("ConcentratePercent", DataType.Double),
                    new("Ref_Done", DataType.Boolean),
                    new("SimID", DataType.String)
                }
            };
        }

        public static class Instances
        {
            public const string R31 = nameof(R31);
            public const string DrumConveyor = "Drum_Conveyor";
        }
        
        public static class Symbols
        {
            public static ArchestraGraphic React => new("React");
            public const string ReactorDisplay = nameof(ReactorDisplay);
        }
    }
}