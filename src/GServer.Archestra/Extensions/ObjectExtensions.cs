using System;
using System.Linq;
using System.Xml.Linq;
using ArchestrA.GRAccess;
using GCommon.Primitives;
using GCommon.Primitives.Base;
using GCommon.Primitives.Enumerations;

// ReSharper disable SuspiciousTypeConversion.Global

namespace GServer.Archestra.Extensions
{
    /// <summary>
    /// ObjectExtensions provides helper methods to make dealing with the IgObject class easier.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Determines if the object is checked out (either to user or someone else).
        /// </summary>
        /// <param name="gObject"></param>
        /// <returns></returns>
        public static bool IsCheckedOut(this IgObject gObject)
        {
            return gObject.CheckoutStatus != ECheckoutStatus.notCheckedOut;
        }

        /// <summary>
        /// Determines if the object is a template using the template naming convention (i.e starts with '$').
        /// </summary>
        /// <param name="gObject"></param>
        /// <returns></returns>
        public static bool IsTemplate(this IgObject gObject)
        {
            return gObject.Tagname.StartsWith("$");
        }

        /// <summary>
        /// Closes the object without saving an changes. If changes do not exist, method will call UndoCheckout. If
        /// Changes exist, then the object will call CheckIn without saving changes.
        /// </summary>
        /// <param name="gObject"></param>
        public static void ForceClose(this IgObject gObject)
        {
            if (gObject.EditStatus == EEditStatus.notBeingEdited)
                gObject.UndoCheckOut();
            else
                gObject.CheckIn("Close object without saving changes");
            
            gObject.CommandResult.Process();
        }
        
        /// <summary>
        /// Saves current changes and checks in the object.
        /// </summary>
        /// <param name="gObject"></param>
        /// <param name="comment">Change comments to attach to check in</param>
        public static void SaveChanges(this IgObject gObject, string comment)
        {
            gObject.Save();
            gObject.CommandResult.Process();
            
            gObject.CheckIn(comment);
            gObject.CommandResult.Process();
        }

        /// <summary>
        /// Deletes the current object from the galaxy. Calls either DeleteTemplate or DeleteInstance accordingly.
        /// </summary>
        /// <param name="gObject"></param>
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

        /// <summary>
        /// Gets the specified attribute from the object.
        /// </summary>
        /// <param name="gObject"></param>
        /// <param name="name">The name of the attribute to retrieve</param>
        /// <returns></returns>
        public static IAttribute GetAttribute(this IgObject gObject, string name)
        {
            return gObject.Attributes[name];
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
            target.SetSecurityClassification(attribute.Security.ToMx());
            target.SetLocked(attribute.Locked.ToMx());

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
                targetAttribute.SetSecurityClassification(sourceAttribute.Security.ToMx());
                targetAttribute.SetLocked(sourceAttribute.Locked.ToMx());
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
                var extensionType = ExtensionType.FromName(type);

                gObject.AddExtensionPrimitive(type, name, isObjectExtension);
                gObject.ConfigureExtension(name, extensionType, source);
            }
        }

        /// <summary>
        /// Transforms the object to a ArchestraObject entity
        /// </summary>
        /// <param name="gObject"></param>
        /// <returns></returns>
        public static ArchestraObject Map(this IgObject gObject)
        {
            return new()
            {
                TagName = gObject.Tagname,
                HierarchicalName = gObject.HierarchicalName,
                ContainedName = gObject.ContainedName,
                ConfigVersion = gObject.ConfigVersion,
                DerivedFromName = gObject.DerivedFrom,
                BasedOnName = gObject.basedOn,
                Category = gObject.category.ToPrimitive(),
                HostName = gObject.Host,
                AreaName = gObject.Area,
                ContainerName = gObject.Container,
                Attributes = gObject.Attributes.Map()
            };
        }

        /// <summary>
        /// Transforms the template object to a ArchestraObject entity
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static ArchestraObject Map(this ITemplate template)
        {
            return template.AsObject().Map();
        }

        /// <summary>
        /// Transforms the instance object to a ArchestraObject entity
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static ArchestraObject Map(this IInstance instance)
        {
            return instance.AsObject().Map();
        }

        /// <summary>
        /// Casts the template object to a IgObject interface
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static IgObject AsObject(this ITemplate template)
        {
            return (IgObject) template;
        }
        
        /// <summary>
        /// Casts the instance object to a IgObject interface
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static IgObject AsObject(this IInstance instance)
        {
            return (IgObject) instance;
        }

        /// <summary>
        /// Casts the object to the type specified by the generic parameter. This should be IInstance or ITemplate!
        /// </summary>
        /// <param name="gObject"></param>
        /// <returns></returns>
        public static T As<T>(this IgObject gObject)
        {
            return (T) gObject;
        }
    }
}