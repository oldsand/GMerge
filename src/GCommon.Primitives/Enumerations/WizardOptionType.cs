using Ardalis.SmartEnum;

namespace GCommon.Primitives.Enumerations
{
    public class WizardOptionType : SmartEnum<WizardOptionType>
    {
        private WizardOptionType(string name, int value) : base(name, value)
        {
        }
        
        public static readonly WizardOptionType Option = new WizardOptionType("Option", 0);
        public static readonly WizardOptionType ChoiceGroup = new WizardOptionType("ChoiceGroup", 1);

    }
}