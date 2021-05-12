// EF Core entity class. Only EF should be instantiating and setting properties.
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace GalaxyMerge.Data.Entities
{
    public class ObjectLookup
    {
        public int ObjectId { get; private set;  }
        public int DerivedFromId { get; private set;  }
        public string TagName { get; private set;  }
    }
}