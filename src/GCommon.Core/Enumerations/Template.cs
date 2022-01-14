using Ardalis.SmartEnum;

namespace GCommon.Core.Enumerations
{
    public abstract class Template : SmartEnum<Template>
    {
        private Template(string name, int value) : base(name, value)
        {
            /*Primitives = PrimitiveLoader.ForTemplate(this);
            Configs = new Dictionary<string, PrimitiveInfo>();
            InitializeConfigs();*/
        }

        public static readonly Template Galaxy = new GalaxyTemplate();
        public static readonly Template AutoImport = new AutoImportTemplate();
        public static readonly Template DiCommon = new DiCommonTemplate();
        public static readonly Template WinPlatform = new WinPlatformTemplate();
        public static readonly Template AppEngine = new AppEngineTemplate();
        public static readonly Template Area = new AreaTemplate();
        public static readonly Template AnalogDevice = new AnalogDeviceTemplate();
        public static readonly Template DdeSuiteLinkClient = new DdeSuiteLinkClientTemplate();
        public static readonly Template DiscreteDevice = new DiscreteDeviceTemplate();
        public static readonly Template InTouchProxy = new InTouchProxyTemplate();
        public static readonly Template OpcClient = new OpcClientTemplate();
        public static readonly Template RedundantDiObject = new RedundantDiObjectTemplate();
        public static readonly Template UserDefined = new UserDefinedTemplate();
        public static readonly Template Symbol = new SymbolTemplate();
        public static readonly Template ViewEngine = new ViewEngineTemplate();
        public static readonly Template InTouchViewApp = new InTouchViewAppTemplate();
        public static readonly Template Sequencer = new SequencerTemplate();
        public static readonly Template ClientControl = new ClientControlTemplate();
        public static readonly Template SqlData = new SqlDataTemplate();

        public abstract ObjectCategory Category { get; }

        #region InternalClasses

        private class GalaxyTemplate : Template
        {
            public GalaxyTemplate() : base("$Galaxy", 1)
            {
            }

            public override ObjectCategory Category => ObjectCategory.Galaxy;
        }

        private class AutoImportTemplate : Template
        {
            public AutoImportTemplate() : base("$_AutoImport", 2)
            {
            }

            public override ObjectCategory Category => ObjectCategory.ApplicationObject;
        }

        private class DiCommonTemplate : Template
        {
            public DiCommonTemplate() : base("$_DiCommon", 3)
            {
            }

            public override ObjectCategory Category => ObjectCategory.ApplicationObject;
        }

        private class WinPlatformTemplate : Template
        {
            public WinPlatformTemplate() : base("$WinPlatform", 4)
            {
            }

            public override ObjectCategory Category => ObjectCategory.PlatformEngine;
        }

        private class AppEngineTemplate : Template
        {
            public AppEngineTemplate() : base("$AppEngine", 5)
            {
            }

            public override ObjectCategory Category => ObjectCategory.ApplicationEngine;
        }

        private class AreaTemplate : Template
        {
            public AreaTemplate() : base("$Area", 6)
            {
            }

            public override ObjectCategory Category => ObjectCategory.Area;
        }

        private class AnalogDeviceTemplate : Template
        {
            public AnalogDeviceTemplate() : base("$AnalogDevice", 7)
            {
            }

            public override ObjectCategory Category => ObjectCategory.ApplicationObject;
        }

        private class DdeSuiteLinkClientTemplate : Template
        {
            public DdeSuiteLinkClientTemplate() : base("$DDESuiteLinkClient", 8)
            {
            }

            public override ObjectCategory Category => ObjectCategory.IoNetwork;
        }

        private class DiscreteDeviceTemplate : Template
        {
            public DiscreteDeviceTemplate() : base("$DiscreteDevice", 9)
            {
            }

            public override ObjectCategory Category => ObjectCategory.ApplicationObject;
        }

        private class InTouchProxyTemplate : Template
        {
            public InTouchProxyTemplate() : base("$InTouchProxy", 10)
            {
            }

            public override ObjectCategory Category => ObjectCategory.IoNetwork;
        }

        private class OpcClientTemplate : Template
        {
            public OpcClientTemplate() : base("$OPCClient", 11)
            {
            }

            public override ObjectCategory Category => ObjectCategory.IoNetwork;
        }

        private class RedundantDiObjectTemplate : Template
        {
            public RedundantDiObjectTemplate() : base("$RedundantDIObject", 12)
            {
            }

            public override ObjectCategory Category => ObjectCategory.Outpost;
        }

        private class UserDefinedTemplate : Template
        {
            public UserDefinedTemplate() : base("$UserDefined", 13)
            {
            }

            public override ObjectCategory Category => ObjectCategory.ApplicationObject;
        }

        private class SymbolTemplate : Template
        {
            public SymbolTemplate() : base("$Symbol", 14)
            {
            }

            public override ObjectCategory Category => ObjectCategory.Symbol;
        }

        private class ViewEngineTemplate : Template
        {
            public ViewEngineTemplate() : base("$ViewEngine", 15)
            {
            }

            public override ObjectCategory Category => ObjectCategory.ViewEngine;
        }

        private class InTouchViewAppTemplate : Template
        {
            public InTouchViewAppTemplate() : base("$InTouchViewApp", 16)
            {
            }

            public override ObjectCategory Category => ObjectCategory.ViewApp;
        }

        private class SequencerTemplate : Template
        {
            public SequencerTemplate() : base("$Sequencer", 17)
            {
            }

            public override ObjectCategory Category => ObjectCategory.ApplicationObject;
        }

        private class ClientControlTemplate : Template
        {
            public ClientControlTemplate() : base("$ClientControl", 18)
            {
            }

            public override ObjectCategory Category => ObjectCategory.Symbol;
        }

        private class SqlDataTemplate : Template
        {
            public SqlDataTemplate() : base("$SQLData", 19)
            {
            }

            public override ObjectCategory Category => ObjectCategory.ApplicationObject;
        }

        #endregion
    }
}