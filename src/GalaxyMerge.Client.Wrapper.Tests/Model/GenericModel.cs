using System;
using System.Collections.Generic;
using System.Linq;

namespace GalaxyMerge.Client.Wrapper.Tests.Model
{
    public class GenericModel
    {
        public GenericModel()
        {
            OtherModels = new List<SubModel>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }
        public double Value { get; set; }
        public DateTime DateTime { get; set; }
        public SomeType Type { get; set; }
        public SubModel SubModel { get; set; }
        public ICollection<SubModel> SubModels { get; set; }
        public ICollection<SubModel> OtherModels { get; }

        public void AddToOther(SubModel item)
        {
            OtherModels.Add(item);
        }
        
        public void RemoveFromOther(SubModel item)
        {
            OtherModels.Remove(item);
        }

        public void ClearOther()
        {
            OtherModels.Clear();
        }

        public SubModel GetOther(string name)
        {
            return OtherModels.FirstOrDefault(x => x.Name == name);
        }
    }
}