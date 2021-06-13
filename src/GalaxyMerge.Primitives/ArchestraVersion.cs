using GalaxyMerge.Core;

namespace GalaxyMerge.Primitives
{
    public abstract class ArchestraVersion : Enumeration
    {
        private ArchestraVersion()
        {
        }

        private ArchestraVersion(int id, string name) : base(id, name)
        {
        }

        protected abstract string GetCdiVersion();

        public static readonly ArchestraVersion Sp2012 = new Version2012();
        public static readonly ArchestraVersion Sp2012P01 = new Version2012P01();
        public static readonly ArchestraVersion Sp2012R2 = new Version2012R2();
        public static readonly ArchestraVersion Sp2012R2P01 = new Version2012R2P01();
        public static readonly ArchestraVersion Sp2012R2P02 = new Version2012R2P02();
        public static readonly ArchestraVersion Sp2012R2P03 = new Version2012R2P03();

        private class Version2012 : ArchestraVersion
        {
            public Version2012() : base(0, "SP2012")
            {
            }

            protected override string GetCdiVersion()
            {
                return "3275.0113.0000.0000";
            }
        }
        
        private class Version2012P01 : ArchestraVersion
        {
            public Version2012P01() : base(1, "SP2012 Patch 01")
            {
            }

            protected override string GetCdiVersion()
            {
                return "3275.113.110.8";
            }
        }
        
        private class Version2012R2 : ArchestraVersion
        {
            public Version2012R2() : base(2, "SP2012R2")
            {
            }

            protected override string GetCdiVersion()
            {
                return "3388.0127.0000.0000";
            }
        }
        
        private class Version2012R2P01 : ArchestraVersion
        {
            public Version2012R2P01() : base(3, "SP2012R2 Patch 01")
            {
            }

            protected override string GetCdiVersion()
            {
                return "3388.0127.0126.0006";
            }
        }
        
        private class Version2012R2P02 : ArchestraVersion
        {
            public Version2012R2P02() : base(4, "SP2012R2 Patch 02")
            {
            }

            protected override string GetCdiVersion()
            {
                return "3388.0127.0212.0011";
            }
        }
        
        private class Version2012R2P03 : ArchestraVersion
        {
            public Version2012R2P03() : base(5, "SP2012R2 Patch 03")
            {
            }

            protected override string GetCdiVersion()
            {
                return "3388.0127.0300.0012";
            }
        }
    }
}