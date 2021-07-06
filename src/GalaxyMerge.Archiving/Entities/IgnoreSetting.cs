// ReSharper disable UnusedAutoPropertyAccessor.Local

using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archiving.Entities
{
    public class IgnoreSetting
    {
        private IgnoreSetting()
        {
        }
        
        public IgnoreSetting(IgnoreType ignoreType, Template template, string ignoreText)
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