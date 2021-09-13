using Ardalis.SmartEnum;

namespace GCommon.Core.Enumerations
{
    public class WizardAssociationType : SmartEnum<WizardAssociationType>
    {
        private WizardAssociationType(string name, int value) : base(name, value)
        {
        }
        
        public static readonly WizardAssociationType Element = new WizardAssociationType("ElementAssociation", 0);
        public static readonly WizardAssociationType Script = new WizardAssociationType("ScriptAssociation", 1);
        public static readonly WizardAssociationType CustomProperty= new WizardAssociationType("CustomPropertyAssociation", 2);

        
    }
}