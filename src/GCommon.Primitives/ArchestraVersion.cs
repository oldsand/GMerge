using System.Linq;
using GCommon.Primitives.Base;

namespace GCommon.Primitives
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

        public static readonly ArchestraVersion SystemPlatform2012 = new Version2012();
        public static readonly ArchestraVersion SystemPlatform2012P1 = new Version2012P1();
        public static readonly ArchestraVersion SystemPlatform2012R2 = new Version2012R2();
        public static readonly ArchestraVersion SystemPlatform2012R2P1 = new Version2012R2P1();
        public static readonly ArchestraVersion SystemPlatform2012R2P2 = new Version2012R2P2();
        public static readonly ArchestraVersion SystemPlatform2012R2P3 = new Version2012R2P3();
        public static readonly ArchestraVersion SystemPlatform2014 = new Version2014();
        public static readonly ArchestraVersion SystemPlatform2014P1 = new Version2014P1();
        public static readonly ArchestraVersion SystemPlatform2014R2 = new Version2014R2();
        public static readonly ArchestraVersion SystemPlatform2014R2P1 = new Version2014R2P1();
        public static readonly ArchestraVersion SystemPlatform2014R2S1 = new Version2014R2S1();
        public static readonly ArchestraVersion SystemPlatform2014R2S1P2 = new Version2014R2S1P2();

        private class Version2012 : ArchestraVersion
        {
            public Version2012() : base(0, "System Platform 2012")
            {
            }

            public override int Number => 44;
            public override string Cdi => "3275.0113.0000.0000";
            public override string Isa => "";
        }

        private class Version2012P1 : ArchestraVersion
        {
            public Version2012P1() : base(1, "SP2012 Patch 01")
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

        private class Version2012R2P1 : ArchestraVersion
        {
            public Version2012R2P1() : base(3, "SP2012R2 Patch 01")
            {
            }

            public override int Number => 0;
            public override string Cdi => "3388.0127.0126.0006";
            public override string Isa => "";
        }

        private class Version2012R2P2 : ArchestraVersion
        {
            public Version2012R2P2() : base(4, "SP2012R2 Patch 02")
            {
            }

            public override int Number => 0;
            public override string Cdi => "3388.0127.0212.0011";
            public override string Isa => "";
        }

        private class Version2012R2P3 : ArchestraVersion
        {
            public Version2012R2P3() : base(5, "SP2012R2 Patch 03")
            {
            }

            public override int Number => 0;
            public override string Cdi => "3388.0127.0300.0012";
            public override string Isa => "";
        }
        
        private class Version2014 : ArchestraVersion
        {
            public Version2014() : base(5, "SP2012R2 Patch 03")
            {
            }

            public override int Number => 0;
            public override string Cdi => "3502.0148.0000.0000";
            public override string Isa => "";
        }
        
        private class Version2014P1 : ArchestraVersion
        {
            public Version2014P1() : base(5, "2014P1")
            {
            }

            public override int Number => 0;
            public override string Cdi => "3509.0148.0140.0007";
            public override string Isa => "";
        }
        
        private class Version2014R2 : ArchestraVersion
        {
            public Version2014R2() : base(5, "2014R2")
            {
            }

            public override int Number => 0;
            public override string Cdi => "3735.0233.0000.0000";
            public override string Isa => "";
        }
        
        private class Version2014R2P1 : ArchestraVersion
        {
            public Version2014R2P1() : base(5, "2014R2P1")
            {
            }

            public override int Number => 0;
            public override string Cdi => "3735.0233.0223.0032";
            public override string Isa => "";
        }
        
        private class Version2014R2S1 : ArchestraVersion
        {
            public Version2014R2S1() : base(5, "2014R2S1")
            {
            }

            public override int Number => 0;
            public override string Cdi => "3735.0233.0399.0061";
            public override string Isa => "";
        }
        
        private class Version2014R2S1P2 : ArchestraVersion
        {
            public Version2014R2S1P2() : base(5, "2014R2S1P1")
            {
            }

            public override int Number => 0;
            public override string Cdi => "3735.0233.0776.0085";
            public override string Isa => "";
        }
    }
}