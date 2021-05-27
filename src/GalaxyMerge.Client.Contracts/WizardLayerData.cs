using System.Collections.Generic;

namespace GalaxyMerge.Contracts
{
    public class WizardLayerData
    {
        public string Name { get; set; }
        public string Rule { get; set; }
        public IEnumerable<WizardAssociationData> Associations { get; set; }
    }
}