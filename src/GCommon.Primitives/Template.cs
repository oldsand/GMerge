using GCommon.Primitives.Base;

namespace GCommon.Primitives
{
    public class Template : Enumeration
    {
        private Template()
        {
        }

        private Template(int id, string name) : base(id, name)
        {
        }
        
        public static readonly Template Galaxy = new Template(1, "$Galaxy"); 
        public static readonly Template AutoImport = new Template(2, "$_AutoImport"); 
        public static readonly Template DiCommon = new Template(3, "$_DiCommon"); 
        public static readonly Template WinPlatform = new Template(4, "$WinPlatform"); 
        public static readonly Template AppEngine = new Template(5, "$AppEngine"); 
        public static readonly Template Area = new Template(6, "$Area"); 
        public static readonly Template AnalogDevice = new Template(7, "$AnalogDevice"); 
        public static readonly Template DdeSuiteLinkClient = new Template(8, "$DDESuiteLinkClient"); 
        public static readonly Template DiscreteDevice = new Template(9, "$DiscreteDevice"); 
        public static readonly Template InTouchProxy = new Template(10, "$InTouchProxy"); 
        public static readonly Template OpcClient = new Template(11, "$OPCClient"); 
        public static readonly Template RedundantDiObject = new Template(12, "$RedundantDIObject"); 
        public static readonly Template UserDefined = new Template(13, "$UserDefined"); 
        public static readonly Template Symbol = new Template(14, "$Symbol"); 
        public static readonly Template ViewEngine = new Template(15, "$ViewEngine"); 
        public static readonly Template InTouchViewApp = new Template(16, "$InTouchViewApp"); 
        public static readonly Template Sequencer = new Template(17, "$Sequencer"); 
        public static readonly Template ClientControl = new Template(18, "$ClientControl"); 
        public static readonly Template SqlData = new Template(19, "$SQLData"); 

    }
}