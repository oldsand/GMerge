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

                //Step1: Update object predefined xml config attributes
                /*UpdateUserAttributes();
                UpdateFieldAttributes();
                UpdateCommandData();*/
                _target.Save();

                //Step2: Configure each user defined attribute and field attribute 
                ConfigureUserAttributes();
                //todo add configure field attributes
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

        /*private void UpdateUserAttributes()
        {
            var source = _source.UserAttributeConfig;
            if (source == null)
                throw new InvalidOperationException(
                    $"Could not obtain user attribute config on source object '{_source.TagName}'");

            var target = _target.GetUaConfig();
            if (target == null)
                throw new InvalidOperationException("Could not find attribute UDAs on target object");
            
            target.SetValue(source);
            target.CommandResult.Process();
        }

        private void UpdateFieldAttributes()
        {
            var source = _source.FieldAttributeConfig;
            if (source == null)
                throw new InvalidOperationException(
                    $"Could not obtain field attribute config on source object '{_source.TagName}'");
            
            var target = _target.GetFaConfig();
            if (target == null)
                throw new InvalidOperationException("Could not find attribute UserAttrData on target object");
            
            target.SetValue(source);
            target.CommandResult.Process();
        }
        
        private void UpdateCommandData()
        {
            var source = _source.CommandDataConfig;
            if (source == null)
                throw new InvalidOperationException(
                    $"Could not obtain command attribute config on source object '{_source.TagName}'");
            
            var target = _target.GetCmdDataConfig();
            if (target == null)
                throw new InvalidOperationException("Could not find attribute CmdData on target object");
            
            target.SetValue(source);
            target.CommandResult.Process();
        }*/
        
        private void ConfigureUserAttributes()
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