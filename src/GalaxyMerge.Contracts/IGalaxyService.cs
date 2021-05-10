using System.Collections.Generic;
using System.ServiceModel;
using GalaxyMerge.Archestra.Entities;

namespace GalaxyMerge.Contracts
{
    [ServiceContract(Namespace = "http://www.gmerge.com/2014/Contracts")]
    public interface IGalaxyService
    {
        [OperationContract]
        bool Connect(string galaxyName);
        
        [OperationContract]
        GalaxyObject GetObject(int objectId);
        
        [OperationContract]
        GalaxyObject GetObjects(string tagName);

        [OperationContract]
        IEnumerable<GalaxyObject> GetObjects(IEnumerable<int> objectIds);
        
        [OperationContract]
        IEnumerable<GalaxyObject> GetObjects(IEnumerable<string> tagNames);
    }
}