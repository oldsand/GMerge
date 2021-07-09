// ReSharper disable InconsistentNaming

using GalaxyMerge.Primitives.Base;

namespace GalaxyMerge.Primitives
{
    public class AttributeCategory : Enumeration
    {
        /// <summary>
        /// Can only be configured at IDE time and not Lockable. Attribute doesn't exist at Runtime.
        /// </summary>
        public static readonly AttributeCategory Undefined = new AttributeCategory(-1, "Undefined");

        /// <summary>
        /// Can only be configured at IDE time and is Lockable. Attribute doesn't exist at Runtime.
        /// </summary>
        public static readonly AttributeCategory PackageOnly = new AttributeCategory(0, "PackageOnly");

        /// <summary>
        /// Not configurable at IDE time. Calculated and readable at runtime, and not Checkpointed.
        /// </summary>
        public static readonly AttributeCategory PackageOnlyLockable = new AttributeCategory(1, "PackageOnlyLockable");

        /// <summary>
        /// Not configurable at IDE time. Calculated and readable at runtime and Checkpointed.
        /// </summary>
        public static readonly AttributeCategory Calculated = new AttributeCategory(2, "Calculated");

        /// <summary>
        /// Only the Security Classification is configurable at IDE time. User writable but not Supervisory writable.Checkpointed.
        /// </summary>
        public static readonly AttributeCategory CalculatedRetentive = new AttributeCategory(3, "CalculatedRetentive");

        /// <summary>
        /// Not configurable at IDE time. Supervisory writable but not User writable.Checkpointed.
        /// </summary>
        public static readonly AttributeCategory Writeable_U = new AttributeCategory(4, "Writeable_U");

        /// <summary>
        /// Only the Security Classification is configurable at IDE time. User and Supervisory writable and Checkpointed.
        /// </summary>
        public static readonly AttributeCategory Writeable_S = new AttributeCategory(5, "Writeable_S");

        /// <summary>
        /// Can be configured at IDE time but not lockable. User writable but not Supervisory writable. Checkpointed.
        /// </summary>
        public static readonly AttributeCategory Writeable_US = new AttributeCategory(6, "Writeable_US");

        /// <summary>
        /// Can be configured at IDE time but not lockable. User and Supervisory writable and Checkpointed.
        /// </summary>
        public static readonly AttributeCategory Writeable_UC = new AttributeCategory(7, "Writeable_UC");

        /// <summary>
        /// Can be configured at IDE time and is Lockable. User writable but not Supervisory writable. Checkpointed.
        /// </summary>
        public static readonly AttributeCategory Writeable_USC = new AttributeCategory(8, "Writeable_USC");

        /// <summary>
        /// Can be configured at IDE time and is Lockable. User and Supervisory writable and Checkpointed.
        /// </summary>
        public static readonly AttributeCategory Writeable_UC_Lockable = new AttributeCategory(9, "Writeable_UC_Lockable");

        /// <summary>
        /// Can be configured at IDE time and is Lockable. Not writable at runtime or checkpointed.
        /// </summary>
        public static readonly AttributeCategory Writeable_USC_Lockable = new AttributeCategory(10, "Writeable_USC_Lockable");

        /// <summary>
        /// Not configurable or Lockable at IDE time. User or Supervisory writable, but System writable. Not checkpointed.
        /// </summary>
        public static readonly AttributeCategory Writeable_C_Lockable = new AttributeCategory(11, "Writeable_C_Lockable");

        /// <summary>
        /// Not configurable or Lockable at IDE time. User or Supervisory writable, but System writable.Not readable at Runtime.Not checkpointed.
        /// </summary>
        public static readonly AttributeCategory SystemSetsOnly = new AttributeCategory(12, "SystemSetsOnly");

        /// <summary>
        /// Not configurable or Lockable, but readable at IDE time. Not writable but readable at Runtime.Not checkpointed.
        /// </summary>
        public static readonly AttributeCategory SystemInternal = new AttributeCategory(13, "SystemInternal");

        /// <summary>
        /// Defined at SDK time and not modified after that.
        /// </summary>
        public static readonly AttributeCategory SystemWriteable = new AttributeCategory(14, "SystemWriteable");

        /// <summary>
        /// Defined at Config time. This attribute does not exist at runtime
        /// </summary>
        public static readonly AttributeCategory Constant = new AttributeCategory(15, "Constant");

        /// <summary>
        /// Defined at SDK time. This attribute does not exist at runtime
        /// </summary>
        public static readonly AttributeCategory SystemInternalBrowsable = new AttributeCategory(16, "SystemInternalBrowsable");

        /// <summary>
        /// Defined at SDK time. This attribute is used to send data to runtime.
        /// </summary>
        public static readonly AttributeCategory CalculatedPackage = new AttributeCategory(17, "CalculatedPackage");

        public static readonly AttributeCategory DeleteAfterStartup = new AttributeCategory(18, "DeleteAfterStartup");
        public static readonly AttributeCategory CategoryEnd = new AttributeCategory(19, "CategoryEnd");

        private AttributeCategory(int id, string name) : base(id, name)
        {
        }
    }
}