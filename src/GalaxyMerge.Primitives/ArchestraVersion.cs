using System.Linq;
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

        public static ArchestraVersion FromCid(string cdi)
        {
            return GetAll<ArchestraVersion>().SingleOrDefault(v => v.Cdi == cdi);
        }
        
        public abstract int Number { get; }
        public abstract string Cdi { get; }
        public abstract string Isa { get; }

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

            public override int Number => 44;
            public override string Cdi => "3275.0113.0000.0000";
            public override string Isa => "";
        }

        private class Version2012P01 : ArchestraVersion
        {
            public Version2012P01() : base(1, "SP2012 Patch 01")
            {
            }

            public override int Number => 0;
            public override string Cdi => "3275.113.110.8";
            public override string Isa => "";
        }

        private class Version2012R2 : ArchestraVersion
        {
            public Version2012R2() : base(2, "SP2012R2")
            {
            }

            public override int Number => 0;
            public override string Cdi => "3388.0127.0000.0000";
            public override string Isa => "";
        }

        private class Version2012R2P01 : ArchestraVersion
        {
            public Version2012R2P01() : base(3, "SP2012R2 Patch 01")
            {
            }

            public override int Number => 0;
            public override string Cdi => "3388.0127.0126.0006";
            public override string Isa => "";
        }

        private class Version2012R2P02 : ArchestraVersion
        {
            public Version2012R2P02() : base(4, "SP2012R2 Patch 02")
            {
            }

            public override int Number => 0;
            public override string Cdi => "3388.0127.0212.0011";
            public override string Isa => "";
        }

        private class Version2012R2P03 : ArchestraVersion
        {
            public Version2012R2P03() : base(5, "SP2012R2 Patch 03")
            {
            }

            public override int Number => 0;
            public override string Cdi => "3388.0127.0300.0012";
            public override string Isa => "";
        }
    }
}