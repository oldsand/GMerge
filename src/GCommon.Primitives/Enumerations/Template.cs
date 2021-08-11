using Ardalis.SmartEnum;

namespace GCommon.Primitives.Enumerations
{
    public class Template : SmartEnum<Template>
    {
        private Template(string name, int value) : base(name, value)
        {
        }
        
        public static readonly Template Galaxy = new Template("$Galaxy", 1); 
        public static readonly Template AutoImport = new Template("$_AutoImport", 2); 
        public static readonly Template DiCommon = new Template("$_DiCommon", 3); 
        public static readonly Template WinPlatform = new Template("$WinPlatform", 4); 
        public static readonly Template AppEngine = new Template("$AppEngine", 5); 
        public static readonly Template Area = new Template("$Area", 6); 
        public static readonly Template AnalogDevice = new Template("$AnalogDevice", 7); 
        public static readonly Template DdeSuiteLinkClient = new Template("$DDESuiteLinkClient", 8); 
        public static readonly Template DiscreteDevice = new Template("$DiscreteDevice", 9); 
        public static readonly Template InTouchProxy = new Template( "$InTouchProxy", 10); 
        public static readonly Template OpcClient = new Template( "$OPCClient", 11); 
        public static readonly Template RedundantDiObject = new Template( "$RedundantDIObject", 12); 
        public static readonly Template UserDefined = new Template( "$UserDefined", 13); 
        public static readonly Template Symbol = new Template( "$Symbol", 14); 
        public static readonly Template ViewEngine = new Template( "$ViewEngine", 15); 
        public static readonly Template InTouchViewApp = new Template( "$InTouchViewApp", 16); 
        public static readonly Template Sequencer = new Template( "$Sequencer", 17); 
        public static readonly Template ClientControl = new Template( "$ClientControl", 18); 
        public static readonly Template SqlData = new Template( "$SQLData", 19);
    }
}