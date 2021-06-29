// EF Core entity class. Only EF should be instantiating and setting properties.
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace GalaxyMerge.Data.Entities
{
    public class ChangeOperation
    {
        private ChangeOperation()
        {
        }
        
        public int OperationId { get; private set; }
        public string OperationName { get; private set; }
    }
}