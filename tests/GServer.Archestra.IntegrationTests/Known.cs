using System.Collections.Generic;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;

namespace GServer.Archestra.IntegrationTests
{
    public static class Known
    {
        public static class Templates
        {
            public static ArchestraObject DrumConveyor => new(
                "$Drum_Conveyor",
                Template.UserDefined,
                configVersion: 86,
                attributes: new List<ArchestraAttribute>
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
            );
            
            public static ArchestraObject ReactorSet => new(
                "$ReactorSet",
                Template.UserDefined,
                configVersion: 94,
                attributes: new List<ArchestraAttribute>
                {
                    new("Auto", DataType.Boolean, security: SecurityClassification.Tune),
                    new("BatchNum", DataType.Integer),
                    new("BatchPhase", DataType.Integer),
                    new("ConcentratePercent", DataType.Double),
                    new("Ref_Done", DataType.Boolean),
                    new("SimID", DataType.String)
                }
            );
        }

        public static class Instances
        {
            public const string R31 = nameof(R31);
            public const string DrumConveyor = "Drum_Conveyor";
        }
        
        public static class Symbols
        {
            public const string ProportionalValve = nameof(ProportionalValve);
            public static ArchestraGraphic React => new("React");
            public const string ReactorDisplay = nameof(ReactorDisplay);
        }
    }
}