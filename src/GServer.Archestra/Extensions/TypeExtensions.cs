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
        public static ValidationStatus ToPrimitiveType(this EPACKAGESTATUS ePackageStatus)
        {
            return (ValidationStatus) ePackageStatus;
        }

        public static EPACKAGESTATUS ToMxType(this ValidationStatus validationStatus)
        {
            return (EPACKAGESTATUS) validationStatus;
        }

        public static ObjectCategory ToPrimitiveType(this ECATEGORY category)
        {
            return ObjectCategory.FromValue((int) category);
        }

        public static ECATEGORY ToMxType(this ObjectCategory category)
        {
            return (ECATEGORY) category.Value;
        }

        public static DataType ToPrimitiveType(this MxDataType mxDataType)
        {
            return DataType.FromValue((int) mxDataType);
        }

        public static MxDataType ToMxType(this DataType dataType)
        {
            return (MxDataType) dataType.Value;
        }

        public static AttributeCategory ToPrimitiveType(this MxAttributeCategory mxAttributeCategory)
        {
            return AttributeCategory.FromValue((int) mxAttributeCategory);
        }

        public static MxAttributeCategory ToMxType(this AttributeCategory attributeCategory)
        {
            return (MxAttributeCategory) attributeCategory.Value;
        }

        public static SecurityClassification ToPrimitiveType(this MxSecurityClassification mxSecurityClassification)
        {
            return SecurityClassification.FromValue((int) mxSecurityClassification);
        }

        public static MxSecurityClassification ToMxType(this SecurityClassification securityClassification)
        {
            return (MxSecurityClassification) securityClassification.Value;
        }

        public static LockType ToPrimitiveType(this MxPropertyLockedEnum mxPropertyLocked)
        {
            return LockType.FromValue((int) mxPropertyLocked);
        }

        public static MxPropertyLockedEnum ToMxType(this LockType lockType)
        {
            return (MxPropertyLockedEnum) lockType.Value;
        }

        public static Reference ToPrimitiveType(this IMxReference mxReference)
        {
            return new()
            {
                FullReference = mxReference.FullReferenceString,
                ObjectReference = mxReference.AutomationObjectReferenceString,
                AttributeReference = mxReference.AttributeReferenceString,
            };
        }

        public static IMxReference ToMxType(this Reference reference)
        {
            var mxValue = new MxValueClass();
            mxValue.PutMxReference(new MxReference());
            var value = mxValue.GetMxReference();
            value.FullReferenceString = reference.FullReference ?? string.Empty;
            value.AutomationObjectReferenceString = reference.ObjectReference ?? string.Empty;
            value.AttributeReferenceString = reference.AttributeReference ?? string.Empty;
            return value;
        }

        public static StatusCategory ToPrimitiveType(this MxStatusCategory mxStatusCategory)
        {
            return (StatusCategory) mxStatusCategory;
        }

        public static MxStatusCategory ToMxType(this StatusCategory statusCategory)
        {
            return (MxStatusCategory) statusCategory;
        }

        public static Quality ToPrimitiveType(this DataQuality mxDataQuality)
        {
            return (Quality) mxDataQuality;
        }

        public static DataQuality ToMxType(this Quality quality)
        {
            return (DataQuality) quality;
        }

        public static CurrentlyDeployedOption ToPrimitiveType(this EActionForCurrentlyDeployedObjects deployedOption)
        {
            return (CurrentlyDeployedOption) deployedOption;
        }

        public static EActionForCurrentlyDeployedObjects ToMxType(this CurrentlyDeployedOption deployedOption)
        {
            return (EActionForCurrentlyDeployedObjects) deployedOption;
        }
    }
}