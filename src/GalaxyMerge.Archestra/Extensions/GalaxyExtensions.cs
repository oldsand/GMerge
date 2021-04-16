using System;
using System.Collections.Generic;
using System.Linq;
using ArchestrA.GRAccess;

namespace GalaxyMerge.Archestra.Extensions
{
    public static class GalaxyExtensions
    {
        /// <summary>
        /// Depth first recursive check-in of all object instances and templates that are descendents of the specified tag name.
        /// This extension provides a simple API for checking in objects without having to worry about checked out descendents,
        /// since the check in call on a single gObject will fail if it has checked out descendents.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="tagName">The base instance or template to check in</param>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
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
            if (template != null) return template.AsGObject();

            var instance = galaxy.GetInstanceByName(tagName);
            return instance.AsGObject();
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

            var template =
                galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsTemplate, new[] {tagName})[tagName];
            ResultHandler.Handle(galaxy.CommandResult, galaxy.Name);
            return template.AsTemplate();
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
            ResultHandler.Handle(galaxy.CommandResult, galaxy.Name);
            return templates;
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
                galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, new[] {tagName})[tagName];
            ResultHandler.Handle(galaxy.CommandResult, galaxy.Name);
            return instance.AsInstance();
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
            ResultHandler.Handle(galaxy.CommandResult, galaxy.Name);
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
            ResultHandler.Handle(galaxy.CommandResult, galaxy.Name);

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
            ResultHandler.Handle(galaxy.CommandResult, galaxy.Name);

            return instances;
        }

        /// <summary>
        /// Gets all objects, both templates and instances, that are descendents, both direct and indirect, of the provided template name.
        /// </summary>
        /// <param name="galaxy">The current galaxy object</param>
        /// <param name="templateName">The base template name.</param>
        /// <returns>IgObjects</returns>
        /// <exception cref="ArgumentNullException">Thrown when tagName is null</exception>
        /// <exception cref="GalaxyException">Thrown when command result is not successful</exception>
        public static IgObjects GetDescendents(this IGalaxy galaxy, string templateName)
        {
            if (templateName == null) throw new ArgumentNullException(nameof(templateName), "templateName cannot be null");

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
        public static bool ObjectIsDescendentOf(this IGalaxy galaxy, string tagName, string templateName)
        {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName), "tagName cannot be null");
            if (templateName == null) throw new ArgumentNullException(nameof(templateName), "templateName cannot be null");
            
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
                ? galaxy.CreateTemplate(tagName, templateName).AsGObject()
                : galaxy.CreateInstance(tagName, templateName).AsGObject();
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
            if (templateName == null) throw new ArgumentNullException(nameof(templateName), "templateName cannot be null");

            tagName = NormalizeTemplateName(tagName);
            
            var parent = galaxy.GetTemplateByName(templateName);
            if (parent == null)
                throw new InvalidOperationException($"Could not find template object with name '{templateName}'");

            var derived = parent.CreateTemplate(tagName);
            ResultHandler.Handle(parent.CommandResult, parent.Tagname);
            
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
            ResultHandler.Handle(parent.CommandResult, parent.Tagname);

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
            ResultHandler.Handle(instances.CommandResults, galaxy.Name);
        }

        public static void AssignToArea(this IGalaxy galaxy, IEnumerable<string> tagNames, string areaName)
        {
            var instances = galaxy.GetInstancesByName(tagNames);
            instances.Area = string.Empty;
            instances.Area = areaName;
            ResultHandler.Handle(instances.CommandResults, galaxy.Name);
        }

        public static void AssignToContainer(this IGalaxy galaxy, IEnumerable<string> tagNames, string containerName)
        {
            var instances = galaxy.GetInstancesByName(tagNames);
            instances.Container = containerName;
            ResultHandler.Handle(instances.CommandResults, galaxy.Name);
        }

        public static void Unassign(this IGalaxy galaxy, IEnumerable<string> tagNames)
        {
            var instances = galaxy.GetInstancesByName(tagNames);
            instances.Host = string.Empty;
            ResultHandler.Handle(instances.CommandResults, galaxy.Name);
            instances.Area = string.Empty;
            ResultHandler.Handle(instances.CommandResults, galaxy.Name);
            instances.Container = string.Empty;
            ResultHandler.Handle(instances.CommandResults, galaxy.Name);
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
            ResultHandler.Handle(gObject.CommandResult, gObject.Tagname);
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
    }
}