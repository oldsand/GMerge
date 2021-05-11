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
        GalaxyObject GetObject(string tagName);

        [OperationContract]
        GalaxySymbol GetSymbol(int objectId);
        
        [OperationContract]
        GalaxySymbol GetSymbol(string tagName);
    }
}