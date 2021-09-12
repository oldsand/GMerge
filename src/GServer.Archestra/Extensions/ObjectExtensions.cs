using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using ArchestrA.GRAccess;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;
using GCommon.Primitives.Structs;
using GServer.Archestra.Helpers;

// ReSharper disable SuspiciousTypeConversion.Global
[assembly: InternalsVisibleTo("GServer.Archestra.IntegrationTests")]

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
                gObject.CheckIn("Closed object without saving changes");
            
            gObject.CommandResult.Process();
        }
        
        /// <summary>
        /// Saves current changes and checks in the object.
        /// </summary>
        /// <param name="gObject"></param>
        /// <param name="comment">Change comments to attach to check in</param>
        public static void SaveAndClose(this IgObject gObject, string comment)
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
        
        /// <summary>
        /// Gets the predefined 'UDAs' attribute from the target object
        /// </summary>
        /// <param name="gObject">The current galaxy object</param>
        /// <returns>IAttribute representing the attribute for the UDAs primitive type</returns>
        /// <remarks>All objects have this primitive attribute which defines the set of user defined attributes on the object.
        /// This attribute can be set to update the object attributes instead of adding each one</remarks>
        public static IAttribute GetUaConfig(this IgObject gObject)
        {
            return gObject.GetAttribute("UDAs");
        }
        
        /// <summary>
        /// Gets the predefined 'UserAttrData' attribute from the target object
        /// </summary>
        /// <param name="gObject">The current galaxy object</param>
        /// <returns>IAttribute representing the attribute for the UserAttrData primitive type</returns>
        /// <remarks>All objects have this primitive attribute which defines the set of field attributes on the object.
        /// This attribute can be set to update the object attributes instead of adding each one</remarks>
        public static IAttribute GetFaConfig(this IgObject gObject)
        {
            return gObject.GetAttribute("UserAttrData");
        }
        
        /// <summary>
        /// Gets the predefined 'CmdData' attribute from the target object
        /// </summary>
        /// <param name="gObject">The current galaxy object</param>
        /// <returns>IAttribute representing the attribute for the CmdData primitive type</returns>
        /// <remarks>All objects have this primitive attribute which defines which attributes have boolean command labels.
        /// This attribute can be set to update the object attributes instead of adding each one</remarks>
        public static IAttribute GetCmdDataConfig(this IgObject gObject)
        {
            return gObject.GetAttribute("CmdData");
        }

        public static IAttribute GetExtensionConfig(this IgObject gObject)
        {
            return gObject.GetAttribute("Extensions");
        }

        public static IEnumerable<string> GetUdaNames(this IgObject gObject)
        {
            var udaData = gObject.GetAttribute("UDAs").GetValue<string>();

            if (udaData == null)
                throw new InvalidOperationException("Not able to fine attribute UDAs");
            
            return XElement.Parse(udaData).Descendants("Attribute")
                    .Select(uda => uda.Attribute("Name")?.Value);
        }
        
        public static Blob GetVisualDefinition(this IgObject gObject, string symbolName = null)
        {
            var name = string.IsNullOrEmpty(symbolName)
                ? "_VisualElementDefinition"
                : $"{symbolName}._VisualElementDefinition";
            
            var attribute = gObject.GetAttribute(name);
            
            return attribute.GetValue<Blob>();
        }

        public static void SetUdaConfig(this IgObject gObject, string xml)
        {
            gObject.Attributes["UDAs"].SetValue(xml);
        }
        
        public static void SetField(this IgObject gObject, string xml)
        {
            gObject.Attributes["UserAttrData"].SetValue(xml);
        }

        public static void ConfigureExtension(this IgObject gObject, string primitiveName, Extension extension, ArchestraObject source)
        {
            var configurableAttributes = extension.GenerateConfigurableAttributes(primitiveName);

            var sourceAttributes = source.Attributes
                .Where(a => configurableAttributes
                    .Any(c => string.Equals(c, a.Name, StringComparison.CurrentCultureIgnoreCase)))
                .OrderBy(a => configurableAttributes.IndexOf(a.Name));

            foreach (var sourceAttribute in sourceAttributes)
            {
                var targetAttribute = gObject.GetAttribute(sourceAttribute.Name);
                targetAttribute.SetValue(sourceAttribute.Value);
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
                var extensionType = Extension.FromName(type);

                gObject.AddExtensionPrimitive(type, name, isObjectExtension);
                gObject.ConfigureExtension(name, extensionType, source);
            }
        }

        /// <summary>
        /// Transforms the object to a ArchestraObject type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ArchestraObject MapObject(this IgObject obj)
        {
            return new ArchestraObject(obj.Tagname,
                obj.HierarchicalName,
                obj.ContainedName,
                obj.ConfigVersion,
                Template.FromName(obj.basedOn),
                obj.DerivedFrom,
                obj.Host,
                obj.Area,
                obj.Container,
                obj.Attributes.Map());
        }

        /// <summary>
        /// Transforms the template object to a ArchestraObject entity
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static ArchestraObject MapObject(this ITemplate template)
        {
            return template.AsObject().MapObject();
        }

        /// <summary>
        /// Transforms the instance object to a ArchestraObject entity
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static ArchestraObject MapObject(this IInstance instance)
        {
            return instance.AsObject().MapObject();
        }
        
        /// <summary>
        /// Transforms the object to a ArchestraGraphic type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ArchestraGraphic MapGraphic(this IgObject obj)
        {
            var definition = obj.GetVisualDefinition();
            var xml = VisualElementConverter.Convert(definition);
            return ArchestraGraphic.Materialize(xml.Root);
        }
        
        public static ArchestraGraphic MapGraphic(this IInstance instance)
        {
            return instance.AsObject().MapGraphic();
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