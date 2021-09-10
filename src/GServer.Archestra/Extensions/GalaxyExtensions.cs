using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.ServiceModel.Security;
using ArchestrA.GRAccess;
using ArchestrA.Visualization.GraphicAccess;
using GCommon.Primitives.Enumerations;
using GServer.Archestra.Exceptions;

[assembly: InternalsVisibleTo("GServer.Archestra.IntegrationTests")]

namespace GServer.Archestra.Extensions
{
    internal static class GalaxyExtensions
    {
        /// <summary>
        /// Logs into the galaxy with the given credentials and validates the user.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="userName">The user name to login with</param>
        /// <param name="password">The password to login with. Recommended to not use password unless galaxy based
        /// security is expected</param>
        /// <exception cref="InvalidOperationException">Thrown when security settings are retrievable</exception>
        /// <exception cref="SecurityAccessDeniedException">Thrown when user is not validated</exception>
        /// <remarks>
        /// In testing the provided Login method, it appears that it does not actually validate the user against Archestra
        /// security settings. In fact, it appears that login will always succeed regardless of the provided parameters.
        /// Login does, however, need to be called prior to any call on the IGalaxy interface.
        /// This extension will login and int turn validate the user name against the galaxy security settings. 
        /// </remarks>
        public static void SecureLogin(this IGalaxy galaxy, string userName, string password = null)
        {
            password ??= string.Empty;
            galaxy.Login(userName, password);
            galaxy.CommandResult.Process();

            var security = galaxy.GetReadOnlySecurity();
            galaxy.CommandResult.Process();

            if (security == null)
                throw new InvalidOperationException(
                    "Could not obtain IGalaxySecurity instance. Unable to authorize user");

            if (security.AuthenticationMode == EAUTHMODE.eNone) return;

            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            // GRAccess interface does not implement IEnumerable
            foreach (IGalaxyUser user in security.UsersAvailable)
                if (user.UserName == userName)
                    return;

            throw new SecurityAccessDeniedException($"User '{userName}' does not have access to '{galaxy.Name}'");
        }

        /// <summary>
        /// Depth first recursive check-in of all object instances and templates that are descendents of the specified
        /// tag name.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="tagName">The base instance or template to check in</param>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        /// <remarks>This extension allows you to perform a hierarchical check in of an object and is descendents
        /// without having to worry about checked out derived templates or instances,
        /// since the  provided CheckIn method on a single gObject will fail if it has checked out descendents.
        /// </remarks>
        public static void DeepCheckIn(this IGalaxy galaxy, string tagName)
        {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName), "Value cannot be null");

            var current = galaxy.GetObjectByName(tagName);

            galaxy.DeepCheckIn(current);
        }

        /// <summary>
        /// Depth first recursive check-in of all object instances and templates that are descendents of the specified tag names.
        /// This extension provides a simple API for checking in objects without having to worry about checked out descendents,
        /// since the check in call on a single gObject will fail if it has checked out descendents.
        /// </summary>
        /// <param name="galaxy">The current galaxy object.</param>
        /// <param name="tagNames">The base instances or templates to check in.</param>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static void DeepCheckIn(this IGalaxy galaxy, IEnumerable<string> tagNames)
        {
            if (tagNames == null) throw new ArgumentNullException(nameof(tagNames), "Value cannot be null");

            var objects = galaxy.GetObjectsByName(tagNames);

            foreach (IgObject gObject in objects)
                galaxy.DeepCheckIn(gObject);
        }

        /// <summary>
        /// Gets the specified object (Instance/Template) from the galaxy repository.
        /// This extension wraps calls into GetTemplateByName and GetInstanceByName to ensure that both types are queried.
        /// This method will also throw an custom GalaxyException when the command result is unsuccessful.
        /// </summary>
        /// <param name="galaxy">The current galaxy object.</param>
        /// <param name="tagName">The tag name to query.</param>
        /// <returns>IgObject</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static IgObject GetObjectByName(this IGalaxy galaxy, string tagName)
        {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName), "Value cannot be null");

            var template = galaxy.GetTemplateByName(tagName);
            if (template != null) return template.AsObject();

            var instance = galaxy.GetInstanceByName(tagName);
            return instance.AsObject();
        }

        /// <summary>
        /// Gets the specified objects (Instance/Template) from the galaxy repository.
        /// This extension wraps calls into GetTemplatesByName and GetInstancesByName to ensure that both types are queried.
        /// This method will also throw an custom GalaxyException when the command result is unsuccessful.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="tagNames">The collection of tag names to query.</param>
        /// <returns>IgObjects</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static IgObjects GetObjectsByName(this IGalaxy galaxy, IEnumerable<string> tagNames)
        {
            if (tagNames == null) throw new ArgumentNullException(nameof(tagNames), "TagNames cannot be null");

            var objectCollection = galaxy.CreategObjectCollection();
            var tags = tagNames.ToArray();

            var templates = galaxy.GetTemplatesByName(tags);
            objectCollection.AddFromCollection(templates);

            var instances = galaxy.GetInstancesByName(tags);
            objectCollection.AddFromCollection(instances);

            return objectCollection;
        }

        /// <summary>
        /// Gets the specified template from the galaxy repository.
        /// This extension wraps the base QueryObjectsByName method of IGalaxy using gObjectIsTemplate to clean up the call.
        /// This method will also throw an custom GalaxyException when the command result is unsuccessful.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="tagName">The tag name to query.</param>
        /// <returns>ITemplate</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static ITemplate GetTemplateByName(this IGalaxy galaxy, string tagName)
        {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName), "Value cannot be null");

            var template = galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsTemplate,
                new[] { tagName })[tagName];

            galaxy.CommandResult.Process();

            return template.As<ITemplate>();
        }

        /// <summary>
        /// Gets the specified base template object from the galaxy repository
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="template">The template to query</param>
        /// <returns></returns>
        public static ITemplate GetBaseTemplate(this IGalaxy galaxy, Template template)
        {
            var baseTemplate =
                galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsTemplate,
                    new[] { template.Name })[template.Name];
            galaxy.CommandResult.Process();

            return baseTemplate.As<ITemplate>();
        }

        /// <summary>
        /// Gets all base template objects from the galaxy repository
        /// </summary>
        /// <param name="galaxy"></param>
        /// <returns></returns>
        public static IgObjects GetBaseTemplates(this IGalaxy galaxy)
        {
            var templateNames = Template.List.Select(t => t.Name).ToArray();

            var templates = galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsTemplate, templateNames);
            galaxy.CommandResult.Process();

            return templates;
        }

        /// <summary>
        /// Gets the specified templates from the galaxy repository.
        /// This extension wraps the base QueryObjectsByName method of IGalaxy using gObjectIsTemplate to clean up the call.
        /// This method will also throw an custom GalaxyException when the command result is unsuccessful.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="tagNames">The collection of tag names to query.</param>
        /// <returns>IgObjects</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static IgObjects GetTemplatesByName(this IGalaxy galaxy, IEnumerable<string> tagNames)
        {
            if (tagNames == null) throw new ArgumentNullException(nameof(tagNames), "Value cannot be null");

            var templates =
                galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsTemplate, tagNames.ToArray());
            galaxy.CommandResult.Process();
            return templates;
        }

        /// <summary>
        /// Gets the specified symbol instance from the galaxy repository
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="tagName">The tag name of the symbol to retrieve</param>
        /// <returns>IInstance when the symbol is found. Null with it is not</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <remarks>Symbol instances must be queried using valid namespace id (3)</remarks>
        public static IInstance GetSymbolByName(this IGalaxy galaxy, string tagName)
        {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName), "Value cannot be null");

            var conditions = galaxy.CreateConditionsObject();
            conditions.Add(EConditionType.NameSpaceIdIs, 3);
            conditions.Add(EConditionType.NameEquals, tagName);
            
            var symbol = galaxy.QueryObjectsMultiCondition(EgObjectIsTemplateOrInstance.gObjectIsInstance,
                conditions)[tagName];
            galaxy.CommandResult.Process();

            return symbol.As<IInstance>();
        }

        /// <summary>
        /// Gets the instance with the provided tag name from the galaxy repository.
        /// This extension wraps the base QueryObjectsByName method of IGalaxy using gObjectIsTemplate to clean up the call.
        /// This method will also throw an custom GalaxyException when the command result is unsuccessful.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="tagName">The tag name to query.</param>
        /// <returns>IInstance</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static IInstance GetInstanceByName(this IGalaxy galaxy, string tagName)
        {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName), "Value cannot be null");

            var instance =
                galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, new[] { tagName })[tagName];
            galaxy.CommandResult.Process();
            return instance.As<IInstance>();
        }

        /// <summary>
        /// Gets all instances with the provided tag names from the galaxy repository.
        /// This extension wraps calls to the base methods of IGalaxy to clean up the calls and provide simpler API.
        /// This method will also throw an custom GalaxyException when the command result is unsuccessful.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="tagNames">The collection of tag names to query.</param>
        /// <returns>IgObjects</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static IgObjects GetInstancesByName(this IGalaxy galaxy, IEnumerable<string> tagNames)
        {
            if (tagNames == null) throw new ArgumentNullException(nameof(tagNames), "Value cannot be null");

            var instances =
                galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, tagNames.ToArray());
            galaxy.CommandResult.Process();
            return instances;
        }

        /// <summary>
        /// Gets all objects, both templates and instances, that are immediate derivations of the provided template name.
        /// This extension wraps calls to the base methods of IGalaxy to clean up the calls and provide simpler API.
        /// This method will also throw an custom GalaxyException when the command result is unsuccessful.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="templateName">The base template name</param>
        /// <returns>IgObjects</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static IgObjects GetDerivedObjects(this IGalaxy galaxy, string templateName)
        {
            if (templateName == null) throw new ArgumentNullException(nameof(templateName), "TagName cannot be null");

            var objectCollection = galaxy.CreategObjectCollection();

            var templates = galaxy.GetDerivedTemplates(templateName);
            objectCollection.AddFromCollection(templates);

            var instances = galaxy.GetDerivedInstances(templateName);
            objectCollection.AddFromCollection(instances);

            return objectCollection;
        }

        /// <summary>
        /// Gets all templates that are immediate derivations of the provided template name.
        /// This extension wraps calls to the base methods of IGalaxy to clean up the calls and provide simpler API.
        /// This method will also throw an custom GalaxyException when the command result is unsuccessful.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="templateName">The base template name</param>
        /// <returns>IgObjects</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static IgObjects GetDerivedTemplates(this IGalaxy galaxy, string templateName)
        {
            if (templateName == null) throw new ArgumentNullException(nameof(templateName), "Value cannot be null");

            var templates = galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsTemplate,
                EConditionType.derivedOrInstantiatedFrom, templateName);
            galaxy.CommandResult.Process();

            return templates;
        }

        /// <summary>
        /// Gets all instances that are immediate derivations of the provided template name.
        /// This extension wraps calls to the base methods of IGalaxy to clean up the calls and provide simpler API.
        /// This method will also throw an custom GalaxyException when the command result is unsuccessful.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="templateName">The base template name</param>
        /// <returns>IgObjects</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static IgObjects GetDerivedInstances(this IGalaxy galaxy, string templateName)
        {
            if (templateName == null) throw new ArgumentNullException(nameof(templateName), "Value cannot be null");

            var instances = galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsInstance,
                EConditionType.derivedOrInstantiatedFrom, templateName);
            galaxy.CommandResult.Process();

            return instances;
        }

        /// <summary>
        /// Gets all objects, both templates and instances, that are derivations (direct or indirect) of the provided
        /// template name.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="templateName">The base template name.</param>
        /// <returns>IgObjects</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static IgObjects GetDescendents(this IGalaxy galaxy, string templateName)
        {
            if (templateName == null)
                throw new ArgumentNullException(nameof(templateName), "templateName cannot be null");

            var collection = galaxy.CreategObjectCollection();

            var instances = galaxy.GetDerivedInstances(templateName);
            collection.AddFromCollection(instances);

            var templates = galaxy.GetDerivedTemplates(templateName);
            collection.AddFromCollection(templates);

            foreach (IgObject template in templates)
                collection.AddFromCollection(galaxy.GetDescendents(template.Tagname));

            return collection;
        }

        /// <summary>
        /// Determines if the object represented by the provided tag name is a descendent of the provided template,
        /// meaning that the object is directly or indirectly derived from the given template.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="tagName">The name of the potential descendent object.</param>
        /// <param name="templateName">The name of the template that the object is potentially derived from.</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName or templateName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static bool IsDescendentOf(this IGalaxy galaxy, string tagName, string templateName)
        {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName), "tagName cannot be null");
            if (templateName == null)
                throw new ArgumentNullException(nameof(templateName), "templateName cannot be null");

            var parent = galaxy.GetObjectByName(templateName);
            if (parent == null)
                throw new InvalidOperationException($"Could not find template object with name '{templateName}'");

            var descendents = galaxy.GetDescendents(parent.Tagname);

            return descendents.Contains(x => x.Tagname == tagName);
        }

        /// <summary>
        /// Creates an object that derives from the provided template. Will create template only if the provided tag name
        /// starts with the standard '$' character. Otherwise will create object as an instance.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="tagName">The name of the object to create.</param>
        /// <param name="templateName">The name of the template that the object derives from.</param>
        /// <returns>IgObject</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName or templateName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static IgObject CreateObject(this IGalaxy galaxy, string tagName, string templateName)
        {
            return tagName.StartsWith("$")
                ? galaxy.CreateTemplate(tagName, templateName).AsObject()
                : galaxy.CreateInstance(tagName, templateName).AsObject();
        }

        /// <summary>
        /// Creates a template that derives from the provided template.
        /// Will prepend '$' if tag name does not start with that character.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="tagName">The name of the template to create.</param>
        /// <param name="templateName">The name of the template that the object derives from.</param>
        /// <returns>ITemplate</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName or templateName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static ITemplate CreateTemplate(this IGalaxy galaxy, string tagName, string templateName)
        {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName), "tagName cannot be null");
            if (templateName == null)
                throw new ArgumentNullException(nameof(templateName), "templateName cannot be null");

            tagName = NormalizeTemplateName(tagName);

            var parent = galaxy.GetTemplateByName(templateName);
            if (parent == null)
                throw new InvalidOperationException($"Could not find template object with name '{templateName}'");

            var derived = parent.CreateTemplate(tagName);
            parent.CommandResult.Process();

            return derived;
        }

        /// <summary>
        /// Creates an instance that derives from the provided template.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="tagName">The name of the instance to create.</param>
        /// <param name="templateName">The name of the template that the object derives from.</param>
        /// <returns>IInstance</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static IInstance CreateInstance(this IGalaxy galaxy, string tagName, string templateName)
        {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName), "tagName cannot be null");

            var parent = galaxy.GetTemplateByName(templateName);
            if (parent == null)
                throw new InvalidOperationException($"Could not find template object with name '{templateName}'");

            var derived = parent.CreateInstance(tagName);
            parent.CommandResult.Process();

            return derived;
        }

        public static IInstance CreateSymbol(this IGalaxy galaxy, string tagName)
        {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName), "tagName cannot be null");

            var parent = galaxy.GetBaseTemplate(Template.Symbol);
            if (parent == null)
                throw new InvalidOperationException($"Could not find template object with name '{Template.Symbol.Name}'");

            var derived = parent.CreateInstance(tagName);
            parent.CommandResult.Process();

            return derived;
        }

        /// <summary>
        /// Depth first recursive delete of all object instances and templates that are descendents of the specified tag name.
        /// This extension provides a simple API for deleting objects without having to worry about descendents,
        /// since deleting a single gObject will fail if it has derived templates or instances.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="tagName">The name of the instance to create.</param>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        public static void DeepDelete(this IGalaxy galaxy, string tagName)
        {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName), "tagName cannot be null");

            var current = galaxy.GetObjectByName(tagName);
            if (current == null) return;

            galaxy.DeepDelete(current);
        }

        public static void AssignToHost(this IGalaxy galaxy, IEnumerable<string> tagNames, string hostName)
        {
            var instances = galaxy.GetInstancesByName(tagNames);
            instances.Host = hostName;
            instances.CommandResults.Process();
        }

        public static void AssignToArea(this IGalaxy galaxy, IEnumerable<string> tagNames, string areaName)
        {
            var instances = galaxy.GetInstancesByName(tagNames);
            instances.Area = string.Empty;
            instances.Area = areaName;
            instances.CommandResults.Process();
        }

        public static void AssignToContainer(this IGalaxy galaxy, IEnumerable<string> tagNames, string containerName)
        {
            var instances = galaxy.GetInstancesByName(tagNames);
            instances.Container = containerName;
            instances.CommandResults.Process();
        }

        public static void UnAssign(this IGalaxy galaxy, IEnumerable<string> tagNames)
        {
            var instances = galaxy.GetInstancesByName(tagNames);

            instances.Host = string.Empty;
            instances.CommandResults.Process();

            instances.Area = string.Empty;
            instances.CommandResults.Process();

            instances.Container = string.Empty;
            instances.CommandResults.Process();
        }

        /// <summary>
        /// Exports object to aaPKG file
        /// </summary>
        /// <param name="galaxy"></param>
        /// <param name="tagName">Name of objects to export</param>
        /// <param name="fileName">Name of file to export to</param>
        ///<remarks>Can only export template and instance objects. Graphics/Symbols are not supported. This is a
        /// limitation of the GRAccess SDK. Make sure to append fileName with .aaPKG</remarks>
        public static void ExportObjects(this IGalaxy galaxy, string tagName, string fileName) =>
            ExportObjectsInternal(galaxy, new[] { tagName }, fileName);

        /// <summary>
        /// Exports objects to aaPKG file
        /// </summary>
        /// <param name="galaxy"></param>
        /// <param name="tagNames">Names of objects to export</param>
        /// <param name="fileName">Name of file to export to</param>
        /// <remarks>Can only export template and instance objects. Graphics/Symbols are not supported. This is a
        /// limitation of the GRAccess SDK. Make sure to append fileName with .aaPKG</remarks>
        public static void ExportObjects(this IGalaxy galaxy, IEnumerable<string> tagNames, string fileName) =>
            ExportObjectsInternal(galaxy, tagNames, fileName);

        public static void ImportObjects(this IGalaxy galaxy, string fileName, bool overwrite)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            galaxy.ImportObjects(fileName, overwrite);
            galaxy.CommandResults.Process();
        }

        public static void ExportGraphic(this IGalaxy galaxy, string tagName, string fileName)
        {
            var access = new GraphicAccess();
            var result = access.ExportGraphicToXml(galaxy, tagName, fileName);
            result.Process();
        }

        public static void ImportGraphic(this IGalaxy galaxy, string fileName, string tagName, bool overwrite)
        {
            var access = new GraphicAccess();
            var result = access.ImportGraphicFromXml(galaxy, tagName, fileName, overwrite);
            result.Process();
        }

        public static void Dump(this IGalaxy galaxy, string tagName, string fileName) =>
            DumpInternal(galaxy, new[] { tagName }, fileName);

        public static void Dump(this IGalaxy galaxy, IEnumerable<string> tagNames, string fileName) =>
            DumpInternal(galaxy, tagNames, fileName);

        public static void Load(this IGalaxy galaxy, string fileName, bool overwrite)
        {
            var mode = overwrite ? GRLoadMode.GRLoadModeUpdate : GRLoadMode.GRLoadModeIgnore;
            galaxy.GRLoad(fileName, mode);
            galaxy.CommandResults.Process();
        }

        /// <summary>
        /// Recursively checks in object and all is it's descendents.
        /// </summary>
        /// <param name="galaxy">The current galaxy object.</param>
        /// <param name="gObject">The object to recursively check in.</param>
        /// <exception cref="ArgumentNullException">Thrown when gObject is null</exception>
        /// /// <exception cref="GalaxyException">Thrown when command result is unsuccessful</exception>
        private static void DeepCheckIn(this IGalaxy galaxy, IgObject gObject)
        {
            if (gObject == null) throw new ArgumentNullException(nameof(gObject), "Value cannot be null");

            var derivations = galaxy.GetDerivedObjects(gObject.Tagname);
            foreach (IgObject derived in derivations)
                galaxy.DeepCheckIn(derived);

            if (!gObject.IsCheckedOut()) return;

            gObject.CheckIn();
            gObject.CommandResult.Process();
        }

        /// <summary>
        /// Recursively deletes object and all of it's descendents.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="gObject">The object to delete</param>
        private static void DeepDelete(this IGalaxy galaxy, IgObject gObject)
        {
            if (gObject == null) return;

            var children = galaxy.GetDerivedObjects(gObject.Tagname);
            foreach (IgObject child in children)
                galaxy.DeepDelete(child);

            gObject.Delete();
        }

        /// <summary>
        /// Prepends the '$' character to the tag name if the string doe not start with it. This is necessary since all
        /// Archestra template objects must start with '$'.
        /// </summary>
        /// <param name="tagName">The tag name to normalize</param>
        /// <returns>string</returns>
        private static string NormalizeTemplateName(string tagName)
        {
            if (!tagName.StartsWith("$"))
                tagName = tagName.Insert(0, "$");
            return tagName;
        }

        /// <summary>
        /// Exports the list of provided objects as an aaPKG file. 
        /// </summary>
        /// <param name="galaxy"></param>
        /// <param name="tagNames">List of tag names to export</param>
        /// <param name="fileName">Destination file name of the aaPKG</param>
        /// <exception cref="InvalidOperationException"></exception>
        private static void ExportObjectsInternal(IGalaxy galaxy, IEnumerable<string> tagNames, string fileName)
        {
            if (tagNames == null) throw new ArgumentNullException(nameof(tagNames));
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            var collection = galaxy.GetObjectsByName(tagNames);

            if (collection == null)
                throw new InvalidOperationException(
                    "Could not find provided tagNames in galaxy. Ensure that the objects are either templates or instances");

            collection.ExportObjects(EExportType.exportAsPDF, fileName);
            collection.CommandResults.Process();
        }

        private static void DumpInternal(IGalaxy galaxy, IEnumerable<string> tagNames, string fileName)
        {
            if (tagNames == null) throw new ArgumentNullException(nameof(tagNames));
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            var collection = galaxy.GetInstancesByName(tagNames);

            if (collection == null)
                throw new InvalidOperationException(
                    "Could not find provided tagNames in galaxy. Ensure that the objects are either templates or instances");

            collection.ExportObjects(EExportType.exportAsCSV, fileName);
            collection.CommandResults.Process();
        }
    }
}