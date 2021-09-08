using System;
using System.Linq;
using System.Runtime.CompilerServices;
using ArchestrA.GRAccess;
using GCommon.Primitives;
using GServer.Archestra.Extensions;

[assembly: InternalsVisibleTo("GServer.Archestra.IntegrationTests")]

namespace GServer.Archestra.Helpers
{
    internal class ObjectBuilder
    {
        private readonly IgObject _target;
        private readonly ArchestraObject _source;

        internal ObjectBuilder(IgObject target, ArchestraObject source)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public void Build()
        {
            try
            {
                //Step0: Checkout.
                _target.CheckOut();

                //Step1: Configure the user defined and field attributes for the objects and save before going further.
                UpdateUda();
                UpdateField();
                _target.Save();

                //Step2: Configure each user defined attribute 
                ConfigureAttributes();
                _target.Save();

                //Step3: Configure each attribute and object extension
                ConfigureExtensions();
                _target.Save();

                //Step4: Done. Checkin. 
                _target.CheckIn("Galaxy Merge Service Configured Object");
            }
            catch (Exception)
            {
                //If we fail, we want to clean up before throwing.
                //Don't leave half configured object in the galaxy (i.e. transaction handling).
                _target.ForceClose();
                _target.Delete();
                throw;
            }
        }
        
        private void UpdateUda()
        {
            var source = _source.GetUda();
            if (source == null)
                throw new InvalidOperationException("Could not find attribute UDAs on target object");

            var target = _target.GetAttribute("UDAs");
            if (target == null)
                throw new InvalidOperationException("Could not find attribute UDAs on target object");
            
            target.SetValue(source);
            target.CommandResult.Process();
        }

        private void UpdateField()
        {
            var source = _source.Attributes.SingleOrDefault(a => a.Name == "UserAttrData")?.Value.ToString();
            if (source == null)
                throw new InvalidOperationException("Could not find attribute UserAttrData on source object");
            
            var target = _target.GetAttribute("UserAttrData");
            if (target == null)
                throw new InvalidOperationException("Could not find attribute UserAttrData on target object");
            
            target.SetValue(source);
            target.CommandResult.Process();
        }
        
        private void ConfigureAttributes()
        {
            var attributeNames = _target.GetUdaNames();

            foreach (var attributeName in attributeNames)
            {
                var source = _source.Attributes.SingleOrDefault(a => a.Name == attributeName);

                if (source == null)
                    throw new InvalidOperationException($"Could not find attribute '{attributeName}' on source object");
                
                var description = _source.Attributes
                    .SingleOrDefault(a => a.Name == $"{attributeName}.Description")?.Value.ToString();
                var engUnits = _source.Attributes
                    .SingleOrDefault(a => a.Name == $"{attributeName}.EngUnits")?.Value.ToString();

                var target = _target.GetAttribute(attributeName);
                
                if (target == null)
                    throw new InvalidOperationException($"Could not find attribute '{attributeName}' on target object");
                
                target.Configure(source, description, engUnits);
            }
        }
        
        private void ConfigureExtensions()
        {
            
            /*foreach (var extension in extensions)
            {
                var name = extension.Attribute("Name")?.Value;
                var type = extension.Attribute("ExtensionType")?.Value;
                var isObjectExtension = extension.Name == "Extension";
                var extensionType = ExtensionType.FromName(type);

                gObject.AddExtensionPrimitive(type, name, isObjectExtension);
                gObject.ConfigureExtension(name, extensionType, source);
            }*/
        }
    }
}