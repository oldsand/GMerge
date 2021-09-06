using System;
using System.Runtime.CompilerServices;
using GCommon.Primitives;
using GServer.Archestra.Abstractions;
using GServer.Archestra.Extensions;

[assembly: InternalsVisibleTo("GServer.Archestra.IntegrationTests")]

namespace GServer.Archestra.Helpers
{
    internal class GalaxyBuilder
    {
        private readonly GalaxyRepository _repository;

        private GalaxyBuilder(GalaxyRepository repository)
        {
            _repository = repository;
        }

        public static GalaxyBuilder On(IGalaxyRepository repository)
        {
            if (!repository.Connected)
                throw new ArgumentException("Provided repository is not actively connection/logged in");

            return new GalaxyBuilder((GalaxyRepository)repository);
        }

        public ObjectBuilder For(ArchestraObject source)
        {
            var target = _repository.Galaxy.GetObjectByName(source.TagName);

            if (target != null) return new ObjectBuilder(target, source);
            
            var created = _repository.Galaxy.CreateObject(source.TagName, source.DerivedFromName);
            return new ObjectBuilder(created, source);
        }
        
        public GraphicBuilder For(ArchestraGraphic source)
        {
            var target = _repository.Galaxy.GetObjectByName(source.TagName);

            if (target != null) return new GraphicBuilder(target);
            
            //var created = _repository.Galaxy.CreateObject(source.TagName);
            return new GraphicBuilder(null);
        }
    }
}