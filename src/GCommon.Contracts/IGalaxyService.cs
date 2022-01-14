using System.ServiceModel;

namespace GCommon.Contracts
{
    [ServiceContract(Namespace = "http://www.gmerge.com/Contracts")]
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