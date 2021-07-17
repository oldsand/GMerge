using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ArchestrA.GRAccess;
using GCommon.Core;
using GCommon.Primitives;
using GCommon.Primitives.Base;
using GServer.Archestra.Entities;

// ReSharper disable SuspiciousTypeConversion.Global

namespace GServer.Archestra.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsCheckedOut(this IgObject gObject)
        {
            return gObject.CheckoutStatus != ECheckoutStatus.notCheckedOut;
        }

        public static bool IsTemplate(this IgObject gObject)
        {
            return gObject.Tagname.StartsWith("$");
        }

        //TODO: This is not complete. Also not sure if it's necessary...
        public static bool IsAssignableTo(this IgObject gObject, IgObject assignee)
        {
            if (assignee.category == ECATEGORY.idxCategoryArea)
                return true;
            if (assignee.category == ECATEGORY.idxCategoryApplicationObject)
                return gObject.category == ECATEGORY.idxCategoryApplicationObject;

            return false;
        }
        
        //TODO: This is not complete/tested. Also not sure if it's necessary...
        public static void Assign(this IgObject gObject, IgObject assignee)
        {
            switch (assignee.category)
            {
                case ECATEGORY.idxCategoryArea:
                    gObject.Area = assignee.Tagname;
                    return;
                case ECATEGORY.idxCategoryApplicationObject:
                    gObject.Container = assignee.Tagname;
                    return;
                default:
                    gObject.Host = assignee.Tagname;
                    break;
            }
        }
        
        public static void Delete(this IgObject gObject)
        {
            if (gObject.IsTemplate())
            {
                gObject.As<ITemplate>().DeleteTemplate();
                gObject.CommandResult.Process();
                return;
            }
            
            gObject.As<IInstance>().DeleteInstance();
            gObject.CommandResult.Process();
        }

        public static IAttribute GetAttribute(this IgObject gObject, string name)
        {
            return gObject.Attributes[name];
        }

        public static IAttribute GetConfigurableAttribute(this IgObject gObject, string name)
        {
            return gObject.ConfigurableAttributes[name];
        }

        public static IEnumerable<IAttribute> GetAttributes(this IgObject gObject, IEnumerable<string> names)
        {
            return names.Select(name => gObject.Attributes[name]);
        }

        public static void SetUserDefinedAttributes(this IgObject gObject, ArchestraObject source)
        {
            var sourceUda = source.Attributes.SingleOrDefault(a => a.Name == "UDAs")?.Value.ToString();
            var targetUda = gObject.Attributes["UDAs"];
            targetUda?.SetValue(sourceUda);
        }

        public static void SetFieldAttributes(this IgObject gObject, ArchestraObject source)
        {
            var sourceField = source.Attributes.SingleOrDefault(a => a.Name == "UserAttrData")?.Value.ToString();
            var targetField = gObject.Attributes["UserAttrData"];
            targetField?.SetValue(sourceField);
        }

        public static void ConfigureAttribute(this IgObject gObject, ArchestraAttribute attribute, string description, string units)
        {
            var target = gObject.GetAttribute(attribute.Name);
            target.SetValue(attribute.Value);
            target.SetSecurityClassification(attribute.Security.ToMxType());
            target.SetLocked(attribute.Locked.ToMxType());

            if (!string.IsNullOrEmpty(description))
                target.Description = description;

            if (!string.IsNullOrEmpty(units))
                target.EngUnits = units;
        }

        public static void ConfigureAttributes(this IgObject gObject, ArchestraObject source)
        {
            var udaData = gObject.GetAttribute("UDAs").GetValue<string>();
            var attributeNames =
                XElement.Parse(udaData)
                    .Descendants("Attribute")
                    .Select(uda => uda.Attribute("Name")?.Value);

            foreach (var attributeName in attributeNames)
            {
                var attribute = source.Attributes.SingleOrDefault(a => a.Name == attributeName);
                var description = source.Attributes
                    .SingleOrDefault(a => a.Name == $"{attributeName}.Description")?.Value.ToString();
                var engUnits = source.Attributes
                    .SingleOrDefault(a => a.Name == $"{attributeName}.EngUnits")?.Value.ToString();

                gObject.ConfigureAttribute(attribute, description, engUnits);
            }
        }

        public static void ConfigureExtension(this IgObject gObject, string primitiveName, ExtensionType extensionType, ArchestraObject source)
        {
            var configurableAttributes = extensionType.GenerateConfigurableAttributes(primitiveName);

            var sourceAttributes = source.Attributes
                .Where(a => configurableAttributes
                    .Any(c => string.Equals(c, a.Name, StringComparison.CurrentCultureIgnoreCase)))
                .OrderBy(a => configurableAttributes.IndexOf(a.Name));

            foreach (var sourceAttribute in sourceAttributes)
            {
                var targetAttribute = gObject.GetAttribute(sourceAttribute.Name);
                targetAttribute.SetValue(sourceAttribute.ArrayCount > 0
                    ? sourceAttribute.Value.ToString().Split(',')
                    : sourceAttribute.Value);
                targetAttribute.SetSecurityClassification(sourceAttribute.Security.ToMxType());
                targetAttribute.SetLocked(sourceAttribute.Locked.ToMxType());
            }
        }

        public static void ConfigureExtensions(this IgObject gObject, ArchestraObject source)
        {
            var sourceExtensions = source.Attributes.SingleOrDefault(a => a.Name == "Extensions")?.Value.ToString();
            if (sourceExtensions == null) return;
            var extensions = XElement.Parse(sourceExtensions).Descendants().Where(e => e.HasAttributes);

            foreach (var extension in extensions)
            {
                var name = extension.Attribute("Name")?.Value;
                var type = extension.Attribute("ExtensionType")?.Value;
                var isObjectExtension = extension.Name == "Extension";
                var extensionType = Enumeration.FromName<ExtensionType>(type);

                gObject.AddExtensionPrimitive(type, name, isObjectExtension);
                gObject.ConfigureExtension(name, extensionType, source);
            }
        }

        public static ArchestraObject Map(this IgObject gObject)
        {
            return new ArchestraObject
            {
                TagName = gObject.Tagname,
                HierarchicalName = gObject.HierarchicalName,
                ContainedName = gObject.ContainedName,
                ConfigVersion = gObject.ConfigVersion,
                DerivedFromName = gObject.DerivedFrom,
                BasedOnName = gObject.basedOn,
                Category = gObject.category.ToPrimitiveType(),
                HostName = gObject.Host,
                AreaName = gObject.Area,
                ContainerName = gObject.Container,
                Attributes = gObject.Attributes.AsGalaxyAttributes()
            };
        }

        public static ArchestraObject Map(this ITemplate template)
        {
            return template.AsObject().Map();
        }

        public static ArchestraObject Map(this IInstance instance)
        {
            return instance.AsObject().Map();
        }

        public static IgObject AsObject(this ITemplate template)
        {
            return (IgObject) template;
        }
        
        public static IgObject AsObject(this IInstance instance)
        {
            return (IgObject) instance;
        }

        public static T As<T>(this IgObject gObject)
        {
            return (T) gObject;
        }
    }
}