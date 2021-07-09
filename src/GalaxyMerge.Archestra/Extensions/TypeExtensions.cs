using ArchestrA.GRAccess;
using GalaxyMerge.Archestra.Options;
using GalaxyMerge.Core;
using GalaxyMerge.Primitives;
using GalaxyMerge.Primitives.Base;

namespace GalaxyMerge.Archestra.Extensions
{
    public static class TypeExtensions
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
            return (ObjectCategory) category;
        }
        
        public static ECATEGORY ToMxType(this ObjectCategory category)
        {
            return (ECATEGORY) category;
        } 
        
        public static DataType ToPrimitiveType(this MxDataType mxDataType)
        {
            return Enumeration.FromId<DataType>((int)mxDataType);
        }
        
        public static MxDataType ToMxType(this DataType dataType)
        {
            return (MxDataType) dataType.Id;
        } 
        
        public static AttributeCategory ToPrimitiveType(this MxAttributeCategory mxAttributeCategory)
        {
            return Enumeration.FromId<AttributeCategory>((int)mxAttributeCategory);
        }
        
        public static MxAttributeCategory ToMxType(this AttributeCategory attributeCategory)
        {
            return (MxAttributeCategory) attributeCategory.Id;
        } 
        
        public static SecurityClassification ToPrimitiveType(this MxSecurityClassification mxSecurityClassification)
        {
            return Enumeration.FromId<SecurityClassification>((int)mxSecurityClassification);
        }
        
        public static MxSecurityClassification ToMxType(this SecurityClassification securityClassification)
        {
            return (MxSecurityClassification) securityClassification.Id;
        }
        
        public static LockType ToPrimitiveType(this MxPropertyLockedEnum mxPropertyLocked)
        {
            return Enumeration.FromId<LockType>((int)mxPropertyLocked);
        }
        
        public static MxPropertyLockedEnum ToMxType(this LockType lockType)
        {
            return (MxPropertyLockedEnum) lockType.Id;
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