using System.Collections.Generic;
using GalaxyMerge.Primitives.Base;

namespace GalaxyMerge.Primitives
{
    public abstract class ExtensionType : Enumeration
    {
        public static readonly ExtensionType Alarm = new AlarmExtensionInternal();
        public static readonly ExtensionType Analog = new AnalogExtensionInternal();
        public static readonly ExtensionType BadValueAlarm = new BadValueAlarmExtensionInternal();
        public static readonly ExtensionType Boolean = new BooleanExtensionInternal();
        public static readonly ExtensionType Display = new DisplayExtensionInternal();
        public static readonly ExtensionType History = new HistoryExtensionInternal();
        public static readonly ExtensionType Input = new InputExtensionInternal();
        public static readonly ExtensionType InputOutput = new InputOutputExtensionInternal();
        public static readonly ExtensionType LogDataChangeEvent = new LogDataChangeEventExtensionInternal();
        public static readonly ExtensionType Output = new OutputExtensionInternal();
        public static readonly ExtensionType Scaling = new ScalingExtensionInternal();
        public static readonly ExtensionType Script = new ScriptExtensionInternal();
        public static readonly ExtensionType Symbol = new SymbolExtensionInternal();

        private ExtensionType()
        {
        }

        private ExtensionType(int id, string name) : base(id, name)
        {
        }

        public abstract List<string> GenerateConfigurableAttributes(string attributeName);
        
        private class AlarmExtensionInternal : ExtensionType
        {
            public AlarmExtensionInternal() : base(0, "AlarmExtension")
            {
            }

            public override List<string> GenerateConfigurableAttributes(string attributeName)
            {
                return new List<string>
                {
                    $"{attributeName}.Priority",
                    $"{attributeName}.Category",
                    $"{attributeName}.DescAttrName",
                    $"{attributeName}.ActiveAlarmState",
                    $"{attributeName}.Alarm.TimeDeadband"
                };
            }
        }

        private class AnalogExtensionInternal : ExtensionType
        {
            public AnalogExtensionInternal() : base(1, "AnalogExtension")
            {
            }

            public override List<string> GenerateConfigurableAttributes(string attributeName)
            {
                return new List<string>
                {
                    //Base attributes
                    $"{attributeName}.DeviationAlarmed",
                    $"{attributeName}.HasStatistics",
                    $"{attributeName}.LevelAlarmed",
                    $"{attributeName}.ROCAlarmed",

                    //HasStatistics
                    $"{attributeName}.Stats.AutoResetOnBadInput",
                    $"{attributeName}.Stats.Reset",
                    $"{attributeName}.Stats.SampleSize",
                    
                    //DeviationAlarmed
                    $"{attributeName}.Dev.Major.Alarmed",
                    $"{attributeName}.Dev.Minor.Alarmed",
                    
                    $"{attributeName}.Dev.Major.Tolerance",
                    $"{attributeName}.Dev.Major.Priority",
                    $"{attributeName}.Dev.Major.DescAttrName",
                    $"{attributeName}.Dev.Minor.Tolerance",
                    $"{attributeName}.Dev.Minor.Priority",
                    $"{attributeName}.Dev.Minor.DescAttrName",
                    
                    $"{attributeName}.Dev.Target",
                    $"{attributeName}.Dev.Deadband",
                    $"{attributeName}.Dev.SettlingPeriod",

                    //LevelAlarmed
                    $"{attributeName}.HiHi.Alarmed",
                    $"{attributeName}.Hi.Alarmed",
                    $"{attributeName}.Lo.Alarmed",
                    $"{attributeName}.LoLo.Alarmed",
                    
                    $"{attributeName}.HiHi.Limit",
                    $"{attributeName}.HiHi.DescAttrName",
                    $"{attributeName}.HiHi.Priority",
                    $"{attributeName}.Hi.Limit",
                    $"{attributeName}.Hi.DescAttrName",
                    $"{attributeName}.Hi.Priority",
                    $"{attributeName}.Lo.Limit",
                    $"{attributeName}.Lo.DescAttrName",
                    $"{attributeName}.Lo.Priority",
                    $"{attributeName}.LoLo.Limit",
                    $"{attributeName}.LoLo.DescAttrName",
                    $"{attributeName}.LoLo.Priority",
                    
                    $"{attributeName}.LevelAlarm.ValueDeadband",
                    $"{attributeName}.LevelAlarm.TimeDeadband",
                    
                    //ROCAlarmed
                    $"{attributeName}.Roc.DecreasingHi.Alarmed",
                    $"{attributeName}.Roc.IncreasingHi.Alarmed",
                    
                    $"{attributeName}.Roc.DecreasingHi.Limit",
                    $"{attributeName}.Roc.DecreasingHi.Priority",
                    $"{attributeName}.Roc.DecreasingHi.DescAttrName",
                    $"{attributeName}.Roc.IncreasingHi.Limit",
                    $"{attributeName}.Roc.IncreasingHi.Priority",
                    $"{attributeName}.Roc.IncreasingHi.DescAttrName",
                    
                    $"{attributeName}.Roc.EvalPeriod",
                    $"{attributeName}.Roc.Rate.Units",
                };
            }
        }

        private class BadValueAlarmExtensionInternal : ExtensionType
        {
            public BadValueAlarmExtensionInternal() : base(2, "BadValueAlarmExtension")
            {
            }

            public override List<string> GenerateConfigurableAttributes(string attributeName)
            {
                return new List<string>
                {
                    $"{attributeName}.Bad.DescAttrName",
                    $"{attributeName}.Bad.Priority"
                };
            }
        }

        private class BooleanExtensionInternal : ExtensionType
        {
            public BooleanExtensionInternal() : base(3, "BooleanExtension")
            {
            }

            public override List<string> GenerateConfigurableAttributes(string attributeName)
            {
                return new List<string>
                {
                    $"{attributeName}.HasStatistics",
                    $"{attributeName}.Stats.AutoResetOnBadInput",
                    $"{attributeName}.Stats.Reset"
                };
            }
        }

        private class DisplayExtensionInternal : ExtensionType
        {
            public DisplayExtensionInternal() : base(4, "DisplayExtension")
            {
            }

            public override List<string> GenerateConfigurableAttributes(string attributeName)
            {
                return new List<string>
                {
                    $"{attributeName}._GraphicDefinitionInternal",
                    $"{attributeName}._Hidden",
                    $"{attributeName}._VisualElementDefinition",
                    $"{attributeName}._VisualElementDefinitionChanged",
                    $"{attributeName}.Description"
                };
            }
        }

        private class HistoryExtensionInternal : ExtensionType
        {
            public HistoryExtensionInternal() : base(5, "HistoryExtension")
            {
            }

            public override List<string> GenerateConfigurableAttributes(string attributeName)
            {
                return new List<string>
                {
                    $"{attributeName}.EnableSwingingDoor",
                    $"{attributeName}.EngUnits",
                    $"{attributeName}.ForceStoragePeriod",
                    $"{attributeName}.Hist.DescAttrName",
                    $"{attributeName}.InterpolationType",
                    $"{attributeName}.RateDeadBand",
                    $"{attributeName}.RolloverValue",
                    $"{attributeName}.SampleCount",
                    $"{attributeName}.TrendHi",
                    $"{attributeName}.TrendLo",
                    $"{attributeName}.ValueDeadBand",
                };
            }
        }

        private class InputExtensionInternal : ExtensionType
        {
            public InputExtensionInternal() : base(6, "InputExtension")
            {
            }

            public override List<string> GenerateConfigurableAttributes(string attributeName)
            {
                return new List<string>
                {
                    $"{attributeName}.InputSource"
                };
            }
        }

        private class InputOutputExtensionInternal : ExtensionType
        {
            public InputOutputExtensionInternal() : base(7, "InputOutputExtension")
            {
            }

            public override List<string> GenerateConfigurableAttributes(string attributeName)
            {
                return new List<string>
                {
                    $"{attributeName}.DiffOutputDest",
                    $"{attributeName}.InputSource",
                    $"{attributeName}.OutputDest"
                };
            }
        }

        private class LogDataChangeEventExtensionInternal : ExtensionType
        {
            public LogDataChangeEventExtensionInternal() : base(8, "LogDataChangeEventExtension")
            {
            }

            public override List<string> GenerateConfigurableAttributes(string attributeName)
            {
                return new List<string>
                {
                    $"{attributeName}.LogDataChangeEvent",
                };
            }
        }

        private class OutputExtensionInternal : ExtensionType
        {
            public OutputExtensionInternal() : base(9, "OutputExtension")
            {
            }

            public override List<string> GenerateConfigurableAttributes(string attributeName)
            {
                return new List<string>
                {
                    $"{attributeName}.OutputDest",
                    $"{attributeName}.OutputEveryScan"
                };
            }
        }

        private class ScalingExtensionInternal : ExtensionType
        {
            public ScalingExtensionInternal() : base(10, "ScalingExtension")
            {
            }

            public override List<string> GenerateConfigurableAttributes(string attributeName)
            {
                return new List<string>
                {
                    $"{attributeName}.ClampEnabled",
                    $"{attributeName}.ConversionMode",
                    $"{attributeName}.EngUnitsMax",
                    $"{attributeName}.EngUnitsMin",
                    $"{attributeName}.EngUnitsRangeMax",
                    $"{attributeName}.EngUnitsRangeMin",
                    $"{attributeName}.RawMax",
                    $"{attributeName}.RawMin"
                };
            }
        }

        private class ScriptExtensionInternal : ExtensionType
        {
            public ScriptExtensionInternal() : base(11, "ScriptExtension")
            {
            }

            public override List<string> GenerateConfigurableAttributes(string attributeName)
            {
                return new List<string>
                {
                    $"{attributeName}._AliasReferenceFlags",
                    $"{attributeName}._Binary",
                    $"{attributeName}._ErrorColumn",
                    $"{attributeName}._ErrorLine",
                    $"{attributeName}._ErrorMessage",
                    $"{attributeName}._ErrorReport",
                    $"{attributeName}._ExternalReferenceFlags",
                    $"{attributeName}._ExternalReferences",
                    $"{attributeName}._Guid",
                    $"{attributeName}._LibraryDependencies",
                    $"{attributeName}.Aliases",
                    $"{attributeName}.AliasReferences",
                    $"{attributeName}.DataChangeDeadband",
                    $"{attributeName}.DeclarationsText",
                    $"{attributeName}.ExecuteText",
                    $"{attributeName}.ExecuteTimeout.Limit",
                    $"{attributeName}.ExecutionError.Alarmed",
                    $"{attributeName}.Expression",
                    $"{attributeName}.OffScanText",
                    $"{attributeName}.OnScanText",
                    $"{attributeName}.RunsAsync",
                    $"{attributeName}.ShutdownText",
                    $"{attributeName}.StartupText",
                    $"{attributeName}.State.Historized",
                    $"{attributeName}.TriggerPeriod",
                    $"{attributeName}.TriggerType",
                        
                    //Execution Alarm
                    $"{attributeName}.DescAttrName",
                    $"{attributeName}.Priority",

                    //Historize Stat
                    $"{attributeName}.State.Description",
                    $"{attributeName}.State.EnableSwingingDoor",
                    $"{attributeName}.State.ForceStoragePeriod",
                    $"{attributeName}.State.InterpolationType",
                    $"{attributeName}.State.RateDeadBand",
                    $"{attributeName}.State.RolloverValue",
                    $"{attributeName}.State.SampleCount",
                    $"{attributeName}.State.TrendHi",
                    $"{attributeName}.State.TrendLo",
                    $"{attributeName}.State.ValueDeadBand"
                };
            }
        }

        private class SymbolExtensionInternal : ExtensionType
        {
            public SymbolExtensionInternal() : base(12, "SymbolExtension")
            {
            }

            public override List<string> GenerateConfigurableAttributes(string attributeName)
            {
                return new List<string>
                {
                    $"{attributeName}._GraphicDefinitionInternal",
                    $"{attributeName}._Hidden",
                    $"{attributeName}._VisualElementDefinition",
                    $"{attributeName}._VisualElementDefinitionChanged",
                    $"{attributeName}.Description"
                };
            }
        }
    }
}