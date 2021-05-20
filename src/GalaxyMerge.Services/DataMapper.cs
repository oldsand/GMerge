using System.Linq;
using GalaxyMerge.Archestra.Entities;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Contracts;

namespace GalaxyMerge.Services
{
    //todo perhaps replace with AutoMapper
    public static class DataMapper
    {
        public static GalaxyObjectData Map(GalaxyObject source)
        {
            return MapGalaxyObject(source);
        }

        public static GalaxySymbolData Map(GalaxySymbol source)
        {
            return MapGalaxySymbol(source);
        }
        
        public static ArchiveObjectData Map(ArchiveObject source)
        {
            return MapArchiveObject(source);
        }
        
        public static ArchiveEntryData Map(ArchiveEntry source)
        {
            return MapArchiveEntry(source);
        }

        private static GalaxyObjectData MapGalaxyObject(GalaxyObject source)
        {
            return new GalaxyObjectData
            {
                TagName = source.TagName,
                ContainedName = source.ContainedName,
                HierarchicalName = source.HierarchicalName,
                ConfigVersion = source.ConfigVersion,
                DerivedFromName = source.DerivedFromName,
                BasedOnName = source.BasedOnName,
                Category = source.Category,
                HostName = source.HostName,
                AreaName = source.AreaName,
                ContainerName = source.ContainerName,
                Attributes = source.Attributes.Select(MapAttribute)
            };
        }

        private static GalaxyAttributeData MapAttribute(GalaxyAttribute source)
        {
            return new GalaxyAttributeData
            {
                Name = source.Name,
                DataType = source.DataType,
                Category = source.Category,
                Security = source.Security,
                Locked = source.Locked,
                Value = source.Value,
                ArrayCount = source.ArrayCount
            };
        }

        private static GalaxySymbolData MapGalaxySymbol(GalaxySymbol source)
        {
            return new GalaxySymbolData
            {
                TagName = source.TagName,
                Root = source.Root,
                CustomProperties = source.CustomProperties.Select(MapCustomProperty),
                PredefinedScripts = source.PredefinedScripts.Select(MapPredefinedScript),
                NamedScripts = source.NamedScripts.Select(MapNamedScript),
                VisualTree = source.VisualTree,
                WizardOptions = source.WizardOptions.Select(MapWizardOption),
                WizardLayers = source.WizardLayers.Select(MapWizardLayer)
            };
        }

        private static CustomPropertyData MapCustomProperty(CustomProperty source)
        {
            return new CustomPropertyData
            {
                Name = source.Name,
                DataType = source.DataType,
                Locked = source.Locked,
                Visibility = source.Visibility,
                Overridden = source.Overridden,
                Expression = source.Expression,
                Description = source.Description
            };
        }

        private static PredefinedScriptData MapPredefinedScript(PredefinedScript source)
        {
            return new PredefinedScriptData
            {
                Name = source.Name,
                Text = source.Text
            };
        }

        private static NamedScriptData MapNamedScript(NamedScript source)
        {
            return new NamedScriptData
            {
                Name = source.Name,
                DeadBand = source.DeadBand,
                Expression = source.Expression,
                Trigger = source.Trigger,
                TriggerPeriod = source.TriggerPeriod,
                Text = source.Text
            };
        }
        
        private static WizardOptionData MapWizardOption(WizardOption source)
        {
            return new WizardOptionData
            {
                Name = source.Name,
                OptionType = source.OptionType,
                Rule = source.Rule,
                Description = source.Description,
                DefaultValue = source.DefaultValue,
                Choices = source.Choices.Select(MapWizardChoice)
            };
        }

        private static WizardChoiceData MapWizardChoice(WizardChoice source)
        {
            return new WizardChoiceData
            {
                Name = source.Name,
                Rule = source.Rule
            };
        }
        
        private static WizardLayerData MapWizardLayer(WizardLayer source)
        {
            return new WizardLayerData
            {
                Name = source.Name,
                Rule = source.Rule,
                Associations = source.Associations.Select(MapWizardAssociation)
            };
        }

        private static WizardAssociationData MapWizardAssociation(WizardAssociation source)
        {
            return new WizardAssociationData
            {
                Name = source.Name,
                AssociationType = source.AssociationType
            };
        }

        private static ArchiveObjectData MapArchiveObject(ArchiveObject source)
        {
            return new ArchiveObjectData
            {
                ObjectId = source.ObjectId,
                TagName = source.TagName,
                Version = source.Version,
                Template = source.Template,
                AddedOn = source.AddedOn,
                ModifiedOn = source.ModifiedOn,
                Entries = source.Entries.Select(MapArchiveEntry)
            };
        }

        private static ArchiveEntryData MapArchiveEntry(ArchiveEntry source)
        {
            return new ArchiveEntryData
            {
                EntryId = source.EntryId,
                ObjectId = source.ObjectId,
                Version = source.Version,
                ArchivedOn = source.ArchivedOn,
                Operation = source.Operation,
                CompressedData = source.CompressedData
            };
        }
    }
}