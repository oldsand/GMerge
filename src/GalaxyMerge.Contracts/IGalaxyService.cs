using System.Collections.Generic;
using System.ServiceModel;
using GalaxyMerge.Archestra.Entities;

namespace GalaxyMerge.Contracts
{
    [ServiceContract(Namespace = "http://www.GalaxyAccess.com/2014/Contracts")]
    public interface IGalaxyService
    {
        [OperationContract]
        GalaxyObject GetObject(string galaxyName, string tagName);

        [OperationContract]
        IEnumerable<GalaxyObject> GetObjects(string galaxyName, IEnumerable<string> tagNames);
        
        [OperationContract]
        void UpdateObject(string galaxyName, GalaxyObject template);

        [OperationContract]
        void UpdateObjects(string galaxyName, IEnumerable<GalaxyObject> templates);
        
        [OperationContract]
        void CreateObject(string galaxyName, string tagName);

        [OperationContract]
        void CreateObject(string galaxyName, IEnumerable<string> tagNames);

        [OperationContract]
        void DeleteObject(string galaxyName, string tagName);

        [OperationContract]
        void DeleteObjects(string galaxyName, IEnumerable<string> tagNames);
    }
}