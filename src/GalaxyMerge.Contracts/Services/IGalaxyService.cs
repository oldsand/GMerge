using System.Collections.Generic;
using System.ServiceModel;
using GalaxyMerge.Contracts.Data;

namespace GalaxyMerge.Contracts.Services
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
        void CreateTemplate(string galaxyName, string tagName);

        [OperationContract]
        void CreateTemplates(string galaxyName, IEnumerable<string> tagNames);

        [OperationContract]
        void DeleteTemplate(string galaxyName, string tagName);

        [OperationContract]
        void DeleteTemplates(string galaxyName, IEnumerable<string> tagNames);
    }
}