using System.Collections.Generic;
using GCommon.Core.Enumerations;

namespace GCommon.Core
{
    
    public class Primitive
    {
        public Primitive()
        {
            
        }

        public int Id { get; set; }

        public Primitive Parent { get; set; }

        public IEnumerable<Primitive> Dependents { get; set; }

        public IEnumerable<ArchestraAttribute> Attributes { get; set; }
        
        //public Dictionary<string, PrimitiveInfo> Configs { get; }
        
        /*public ArchestraAttribute GetPrimitive(string name)
        {
            return Primitives.SingleOrDefault(
                x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        public void SetPrimitives(IEnumerable<ArchestraAttribute> primitives)
        {
            Primitives = primitives ?? throw new ArgumentNullException(nameof(primitives));
        }

        public void SetPrimitiveValue(object value, string name) =>
            SetPrimitiveValueInternal(value, name, (a, v) => a.Value = v);

        public void SetPrimitiveValue(object value, string name, Action<ArchestraAttribute, object> setter)
            => SetPrimitiveValueInternal(value, name, setter);

        private void SetPrimitiveValueInternal(object value, string name, Action<ArchestraAttribute, object> setter)
        {
            var primitive = Primitives.SingleOrDefault(x =>
                string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (primitive != null)
                setter.Invoke(primitive, value);
        }

        private void InitializeConfigs()
        {
            Configs.Add(UserAttributeInfo.AttributeName,
                new UserAttributeInfo(GetPrimitive(UserAttributeInfo.AttributeName)));
            Configs.Add(CommandDataInfo.AttributeName,
                new CommandDataInfo(GetPrimitive(CommandDataInfo.AttributeName)));
            Configs.Add(ExtensionInfo.AttributeName, 
                new ExtensionInfo(GetPrimitive(ExtensionInfo.AttributeName)));
        }*/
    }
}