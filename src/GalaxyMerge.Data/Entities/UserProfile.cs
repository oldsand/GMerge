// EF Core entity class. Only EF should be instantiating and setting properties.
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

using System;

namespace GalaxyMerge.Data.Entities
{
    public class UserProfile
    {
        private UserProfile()
        {
        }
        
        public int UserId { get; private set; }
        public Guid UserGuid { get; private set; }
        public string UserName { get; private set; }
        public string UserFullName { get; private set; }
        public string RolesData { get; private set; }
    }
}