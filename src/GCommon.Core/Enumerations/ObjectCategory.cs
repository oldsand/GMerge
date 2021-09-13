using Ardalis.SmartEnum;

namespace GCommon.Core.Enumerations
{
    public class ObjectCategory : SmartEnum<ObjectCategory>
    {
        private ObjectCategory(string name, int value) : base(name, value)
        {
        }
        
        public static readonly ObjectCategory Undefined = new ObjectCategory("Undefined", 0);
        public static readonly ObjectCategory PlatformEngine = new ObjectCategory("PlatformEngine", 1);
        public static readonly ObjectCategory ClusterEngine = new ObjectCategory("ClusterEngine", 2);
        public static readonly ObjectCategory ApplicationEngine = new ObjectCategory("ApplicationEngine", 3);
        public static readonly ObjectCategory ViewEngine = new ObjectCategory("ViewEngine", 4);
        public static readonly ObjectCategory ProductEngine = new ObjectCategory("ProductEngine", 5);
        public static readonly ObjectCategory HistoryEngine = new ObjectCategory("HistoryEngine", 6);
        public static readonly ObjectCategory PrintEngine = new ObjectCategory("PrintEngine", 7);
        public static readonly ObjectCategory Outpost = new ObjectCategory("Outpost", 8);
        public static readonly ObjectCategory QueryEngine = new ObjectCategory("QueryEngine", 9);
        public static readonly ObjectCategory ApplicationObject = new ObjectCategory( "ApplicationObject", 10);
        public static readonly ObjectCategory IoNetwork = new ObjectCategory( "IoNetwork", 11);
        public static readonly ObjectCategory IoDevice = new ObjectCategory( "IoDevice", 12);
        public static readonly ObjectCategory Area = new ObjectCategory( "Area", 13);
        public static readonly ObjectCategory UserProfile = new ObjectCategory( "UserProfile", 14);
        public static readonly ObjectCategory Display = new ObjectCategory( "Display", 15);
        public static readonly ObjectCategory Symbol = new ObjectCategory( "Symbol", 16);
        public static readonly ObjectCategory ViewApp = new ObjectCategory( "ViewApp", 17);
        public static readonly ObjectCategory ProductionObject = new ObjectCategory( "ProductionObject", 18);
        public static readonly ObjectCategory Report = new ObjectCategory( "Report", 19);
        public static readonly ObjectCategory SharedProcedure = new ObjectCategory( "SharedProcedure", 20);
        public static readonly ObjectCategory InsertablePrimitive = new ObjectCategory( "InsertablePrimitive", 21);
        public static readonly ObjectCategory IdeMacro = new ObjectCategory( "IdeMacro", 22);
        public static readonly ObjectCategory Galaxy = new ObjectCategory( "Galaxy", 23);
    }
}