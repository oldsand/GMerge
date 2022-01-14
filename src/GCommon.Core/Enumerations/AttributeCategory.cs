// ReSharper disable InconsistentNaming

using Ardalis.SmartEnum;

namespace GCommon.Core.Enumerations
{
    /// <summary>
    /// All known attribute categories for Archestra object attributes.
    /// <list type="dash">
    /// <item>
    /// <description>Can only be configured at IDE time and not Lockable. Attribute doesn't exist at Runtime.</description>
    /// </item>
    /// </list>
    /// </summary>
    public class AttributeCategory : SmartEnum<AttributeCategory>
    {
        private AttributeCategory(string name, int value) : base(name, value)
        {
        }
        
        public static readonly AttributeCategory Undefined = new AttributeCategory( "Undefined", -1);
        public static readonly AttributeCategory PackageOnly = new AttributeCategory("PackageOnly", 0);
        public static readonly AttributeCategory PackageOnlyLockable = new AttributeCategory("PackageOnlyLockable", 1);
        public static readonly AttributeCategory Calculated = new AttributeCategory("Calculated", 2);
        public static readonly AttributeCategory CalculatedRetentive = new AttributeCategory("CalculatedRetentive", 3);
        public static readonly AttributeCategory Writeable_U = new AttributeCategory("Writeable_U", 4);
        public static readonly AttributeCategory Writeable_S = new AttributeCategory("Writeable_S", 5);
        public static readonly AttributeCategory Writeable_US = new AttributeCategory("Writeable_US", 6);
        public static readonly AttributeCategory Writeable_UC = new AttributeCategory("Writeable_UC", 7);
        public static readonly AttributeCategory Writeable_USC = new AttributeCategory("Writeable_USC", 8);
        public static readonly AttributeCategory Writeable_UC_Lockable = new AttributeCategory("Writeable_UC_Lockable", 9);
        public static readonly AttributeCategory Writeable_USC_Lockable = new AttributeCategory( "Writeable_USC_Lockable", 10);
        public static readonly AttributeCategory Writeable_C_Lockable = new AttributeCategory( "Writeable_C_Lockable", 11);
        public static readonly AttributeCategory SystemSetsOnly = new AttributeCategory( "SystemSetsOnly", 12);
        public static readonly AttributeCategory SystemInternal = new AttributeCategory( "SystemInternal", 13);
        public static readonly AttributeCategory SystemWriteable = new AttributeCategory( "SystemWriteable", 14);
        public static readonly AttributeCategory Constant = new AttributeCategory( "Constant", 15);
        public static readonly AttributeCategory SystemInternalBrowsable = new AttributeCategory( "SystemInternalBrowsable", 16);
        public static readonly AttributeCategory CalculatedPackage = new AttributeCategory( "CalculatedPackage", 17);
        public static readonly AttributeCategory DeleteAfterStartup = new AttributeCategory( "DeleteAfterStartup", 18);
        public static readonly AttributeCategory CategoryEnd = new AttributeCategory( "CategoryEnd", 19);
        
    }
}