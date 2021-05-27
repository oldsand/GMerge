using System.ServiceModel;
using GalaxyMerge.Contracts;

namespace GalaxyMerge.Client.Contracts
{
    [ServiceContract(Namespace = "http://www.gmerge.com/2014/Contracts")]
    public interface IGalaxyService
    {
        [OperationContract]
        bool Connect(string galaxyName);
        
        [OperationContract]
        GalaxyObjectData GetObjectById(int objectId);
        
        [OperationContract]
        GalaxyObjectData GetObjectByName(string tagName);

        [OperationContract]
        GalaxySymbolData GetSymbolById(int objectId);
        
        [OperationContract]
        GalaxySymbolData GetSymbolByName(string tagName);
    }
}