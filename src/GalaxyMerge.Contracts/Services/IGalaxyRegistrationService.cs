using System.ServiceModel;

namespace GalaxyMerge.Contracts.Services
{
    [ServiceContract(Namespace = "http://www.GalaxyAccess.com/2014/Contracts")]
    public interface IGalaxyRegistrationService
    {
        [OperationContract]
        void RegisterGalaxy(string galaxyName);
        
        [OperationContract]
        void UnregisterGalaxy(string galaxyName);
    }
}