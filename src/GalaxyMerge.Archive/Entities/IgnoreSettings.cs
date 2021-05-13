// ReSharper disable UnusedAutoPropertyAccessor.Local
using GalaxyMerge.Common.Primitives;

namespace GalaxyMerge.Archive.Entities
{
    public class IgnoreSettings
    {
        private IgnoreSettings()
        {
        }
        
        public IgnoreSettings(IgnoreType ignoreType, Template template, string ignoreText)
        {
            IgnoreType = ignoreType;
            Template = template;
            IgnoreText = ignoreText;
        }
        
        public int IgnoreId { get; private set;  }
        public IgnoreType IgnoreType { get; private set;  }
        public Template Template { get; private set;  }
        public string IgnoreText { get; private set;  }
    }
}