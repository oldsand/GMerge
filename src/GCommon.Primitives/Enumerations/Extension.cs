using System.Collections.Generic;
using Ardalis.SmartEnum;
using GCommon.Primitives.Helpers;

namespace GCommon.Primitives.Enumerations
{
    public abstract class Extension : SmartEnum<Extension>
    {
        private Extension(string name, int value) : base(name, value)
        {
            Primitives = PrimitiveLoader.ForExtension(this);
        }
        
        public static readonly Extension Alarm = new AlarmExtension();
        public static readonly Extension Analog = new AnalogExtension();
        public static readonly Extension BadValueAlarm = new BadValueAlarmExtension();
        public static readonly Extension Boolean = new BooleanExtension();
        public static readonly Extension Display = new DisplayExtension();
        public static readonly Extension History = new HistoryExtension();
        public static readonly Extension Input = new InputExtension();
        public static readonly Extension InputOutput = new InputOutputExtension();
        public static readonly Extension LogDataChangeEvent = new LogDataChangeEventExtension();
        public static readonly Extension Output = new OutputExtension();
        public static readonly Extension Scaling = new ScalingExtension();
        public static readonly Extension Script = new ScriptExtension();
        public static readonly Extension Symbol = new SymbolExtension();

        public virtual ExtensionType Type => ExtensionType.Attribute;
        public IEnumerable<ArchestraAttribute> Primitives { get; private set; }

        public abstract List<string> GenerateConfigurableAttributes(string attributeName);


        #region InternalClasses

        private class AlarmExtension : Extension
        {
            public AlarmExtension() : base("AlarmExtension", 0)
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

        private class AnalogExtension : Extension
        {
            public AnalogExtension() : base("AnalogExtension", 1)
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

        private class BadValueAlarmExtension : Extension
        {
            public BadValueAlarmExtension() : base("BadValueAlarmExtension", 2)
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

        private class BooleanExtension : Extension
        {
            public BooleanExtension() : base("BooleanExtension", 3)
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

        private class DisplayExtension : Extension
        {
            public DisplayExtension() : base("DisplayExtension", 4)
            {
            }

            public override ExtensionType Type => ExtensionType.Object;

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

        private class HistoryExtension : Extension
        {
            public HistoryExtension() : base("HistoryExtension", 5)
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

        private class InputExtension : Extension
        {
            public InputExtension() : base("InputExtension", 6)
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

        private class InputOutputExtension : Extension
        {
            public InputOutputExtension() : base("InputOutputExtension", 7)
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

        private class LogDataChangeEventExtension : Extension
        {
            public LogDataChangeEventExtension() : base("LogDataChangeEventExtension", 8)
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

        private class OutputExtension : Extension
        {
            public OutputExtension() : base("OutputExtension", 9)
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

        private class ScalingExtension : Extension
        {
            public ScalingExtension() : base( "ScalingExtension", 10)
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

        private class ScriptExtension : Extension
        {
            public ScriptExtension() : base( "ScriptExtension", 11)
            {
            }
            
            public override ExtensionType Type => ExtensionType.Object;

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

        private class SymbolExtension : Extension
        {
            public SymbolExtension() : base( "SymbolExtension", 12)
            {
            }
            
            public override ExtensionType Type => ExtensionType.Object;

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

        #endregion
        
    }
}