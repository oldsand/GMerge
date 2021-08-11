using System.Collections.Generic;
using Ardalis.SmartEnum;

namespace GCommon.Primitives.Enumerations
{
    public abstract class ExtensionType : SmartEnum<ExtensionType>
    {
        private ExtensionType(string name, int value) : base(name, value)
        {
        }
        
        public static readonly ExtensionType Alarm = new AlarmExtensionType();
        public static readonly ExtensionType Analog = new AnalogExtensionType();
        public static readonly ExtensionType BadValueAlarm = new BadValueAlarmExtensionType();
        public static readonly ExtensionType Boolean = new BooleanExtensionType();
        public static readonly ExtensionType Display = new DisplayExtensionType();
        public static readonly ExtensionType History = new HistoryExtensionType();
        public static readonly ExtensionType Input = new InputExtensionType();
        public static readonly ExtensionType InputOutput = new InputOutputExtensionType();
        public static readonly ExtensionType LogDataChangeEvent = new LogDataChangeEventExtensionType();
        public static readonly ExtensionType Output = new OutputExtensionType();
        public static readonly ExtensionType Scaling = new ScalingExtensionType();
        public static readonly ExtensionType Script = new ScriptExtensionType();
        public static readonly ExtensionType Symbol = new SymbolExtensionType();

        public abstract List<string> GenerateConfigurableAttributes(string attributeName);
        
        private class AlarmExtensionType : ExtensionType
        {
            public AlarmExtensionType() : base("AlarmExtension", 0)
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

        private class AnalogExtensionType : ExtensionType
        {
            public AnalogExtensionType() : base("AnalogExtension", 1)
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

        private class BadValueAlarmExtensionType : ExtensionType
        {
            public BadValueAlarmExtensionType() : base("BadValueAlarmExtension", 2)
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

        private class BooleanExtensionType : ExtensionType
        {
            public BooleanExtensionType() : base("BooleanExtension", 3)
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

        private class DisplayExtensionType : ExtensionType
        {
            public DisplayExtensionType() : base("DisplayExtension", 4)
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

        private class HistoryExtensionType : ExtensionType
        {
            public HistoryExtensionType() : base("HistoryExtension", 5)
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

        private class InputExtensionType : ExtensionType
        {
            public InputExtensionType() : base("InputExtension", 6)
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

        private class InputOutputExtensionType : ExtensionType
        {
            public InputOutputExtensionType() : base("InputOutputExtension", 7)
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

        private class LogDataChangeEventExtensionType : ExtensionType
        {
            public LogDataChangeEventExtensionType() : base("LogDataChangeEventExtension", 8)
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

        private class OutputExtensionType : ExtensionType
        {
            public OutputExtensionType() : base("OutputExtension", 9)
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

        private class ScalingExtensionType : ExtensionType
        {
            public ScalingExtensionType() : base( "ScalingExtension", 10)
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

        private class ScriptExtensionType : ExtensionType
        {
            public ScriptExtensionType() : base( "ScriptExtension", 11)
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

        private class SymbolExtensionType : ExtensionType
        {
            public SymbolExtensionType() : base( "SymbolExtension", 12)
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