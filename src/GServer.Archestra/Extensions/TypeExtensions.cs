using ArchestrA.GRAccess;
using GCommon.Primitives;
using GCommon.Primitives.Base;
using GCommon.Primitives.Enumerations;
using GServer.Archestra.Internal;
using GServer.Archestra.Options;

namespace GServer.Archestra.Extensions
{
    internal static class TypeExtensions
    {
        public static ValidationStatus ToPrimitive(this EPACKAGESTATUS ePackageStatus)
        {
            return ValidationStatus.FromValue((int) ePackageStatus);
        }

        public static EPACKAGESTATUS ToMx(this ValidationStatus validationStatus)
        {
            return (EPACKAGESTATUS) validationStatus.Value;
        }

        /// <summary>
        /// Convert the GRAccess API ECATEGORY to the primitive library type
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static ObjectCategory ToPrimitive(this ECATEGORY category)
        {
            return ObjectCategory.FromValue((int) category);
        }

        public static ECATEGORY ToMx(this ObjectCategory category)
        {
            return (ECATEGORY) category.Value;
        }

        public static DataType ToPrimitive(this MxDataType mxDataType)
        {
            return DataType.FromValue((int) mxDataType);
        }

        public static MxDataType ToMx(this DataType dataType)
        {
            return (MxDataType) dataType.Value;
        }

        public static AttributeCategory ToPrimitive(this MxAttributeCategory mxAttributeCategory)
        {
            return AttributeCategory.FromValue((int) mxAttributeCategory);
        }

        public static MxAttributeCategory ToMx(this AttributeCategory attributeCategory)
        {
            return (MxAttributeCategory) attributeCategory.Value;
        }

        public static SecurityClassification ToPrimitive(this MxSecurityClassification mxSecurityClassification)
        {
            return SecurityClassification.FromValue((int) mxSecurityClassification);
        }

        public static MxSecurityClassification ToMx(this SecurityClassification securityClassification)
        {
            return (MxSecurityClassification) securityClassification.Value;
        }

        public static LockType ToPrimitive(this MxPropertyLockedEnum mxPropertyLocked)
        {
            return LockType.FromValue((int) mxPropertyLocked);
        }

        public static MxPropertyLockedEnum ToMx(this LockType lockType)
        {
            return (MxPropertyLockedEnum) lockType.Value;
        }

        public static Reference ToPrimitive(this IMxReference mxReference)
        {
            return new()
            {
                FullReference = mxReference.FullReferenceString,
                ObjectReference = mxReference.AutomationObjectReferenceString,
                AttributeReference = mxReference.AttributeReferenceString,
            };
        }

        public static IMxReference ToMx(this Reference reference)
        {
            var mxValue = new MxValueClass();
            mxValue.PutMxReference(new MxReference());
            var value = mxValue.GetMxReference();
            value.FullReferenceString = reference.FullReference ?? string.Empty;
            value.AutomationObjectReferenceString = reference.ObjectReference ?? string.Empty;
            value.AttributeReferenceString = reference.AttributeReference ?? string.Empty;
            return value;
        }

        public static StatusCategory ToPrimitive(this MxStatusCategory mxStatusCategory)
        {
            return StatusCategory.FromValue((int) mxStatusCategory);
        }

        public static MxStatusCategory ToMx(this StatusCategory statusCategory)
        {
            return (MxStatusCategory) statusCategory.Value;
        }

        public static Quality ToPrimitive(this DataQuality mxDataQuality)
        {
            return Quality.FromValue((int) mxDataQuality);
        }

        public static DataQuality ToMx(this Quality quality)
        {
            return (DataQuality) quality.Value;
        }

        public static CurrentlyDeployedOption ToPrimitive(this EActionForCurrentlyDeployedObjects deployedOption)
        {
            return (CurrentlyDeployedOption) deployedOption;
        }

        public static EActionForCurrentlyDeployedObjects ToMx(this CurrentlyDeployedOption deployedOption)
        {
            return (EActionForCurrentlyDeployedObjects) deployedOption;
        }
    }
}