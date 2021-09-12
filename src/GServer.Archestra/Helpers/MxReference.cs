using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ArchestrA.GRAccess;
using GCommon.Primitives.Structs;

[assembly: InternalsVisibleTo("GServer.Archestra.UnitTests")]

namespace GServer.Archestra.Helpers
{
    
    /// <summary>
    /// Since GRAccess dll doesn't give us a way to create a reference type, I am implementing the interface just to set
    /// the public properties and pass into MxValue Put method in order to actually set reference values.
    /// Class only used for transferring data and has no implementation.
    /// </summary>
    ///  [TypeLibType(2)]
    internal class MxReference : IMxReference
    {
        /// <summary>
        /// Creates an instance of an IMxReference with the provided reference string
        /// </summary>
        /// <param name="fullReferenceString"></param>
        /// <returns>IMxReference</returns>
        public static IMxReference Create(string fullReferenceString)
        {
            //Seemingly the only way to get and instance created is via the MxValueClass.
            //This is why this looks weird. I have to create and use PutMxReference with the concrete implementation,
            //then get the value back out via GetMxReference in order to actually set the reference strings.
            var mxValue = new MxValueClass();
            mxValue.PutMxReference(new MxReference());
            
            var value = mxValue.GetMxReference();
            value.FullReferenceString = fullReferenceString ?? string.Empty;

            return value;
        }
        
        /// <summary>
        /// Creates an instance of an IMxReference with the provided Reference struct
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static IMxReference Create(Reference reference)
        {
            //Seemingly the only way to get and instance created is via the MxValueClass.
            //This is why this looks weird. I have to create and use PutMxReference with the concrete implementation,
            //then get the value back out viw GetMxReference in order to actually set the reference strings.
            var mxValue = new MxValueClass();
            mxValue.PutMxReference(new MxReference());
            
            var value = mxValue.GetMxReference();
            value.FullReferenceString = reference.FullName ?? string.Empty;
            value.AutomationObjectReferenceString = reference.ObjectName ?? string.Empty;
            value.AttributeReferenceString = reference.AttributeName ?? string.Empty;

            return value;
        }
        
        void IPersist.GetClassID(out Guid pClassID)
        {
            throw new NotImplementedException();
        }

        void IMxReference.IsDirty()
        {
        }

        void IMxReference.Load(IStream pstm)
        {
        }

        void IMxReference.Save(IStream pstm, int fClearDirty)
        {
        }

        void IMxReference.GetSizeMax(out _ULARGE_INTEGER pcbSize)
        {
            throw new NotImplementedException();
        }

        public IMxReference GenerateEnumStringsReference(MxValue pMxValue)
        {
            throw new NotImplementedException();
        }

        public string FullReferenceString { get; set; }
        public string AutomationObjectReferenceString { get; set; }
        public string AttributeReferenceString { get; set; }
        public MxResolutionStatus AutomationObjectResolutionStatus { get; }
        public MxResolutionStatus AttributeResolutionStatus { get; }
        public string Context { get; set; }

        void IMxReference.GetClassID(out Guid pClassID)
        {
            throw new NotImplementedException();
        }

        void IPersistStream.IsDirty()
        {
            throw new NotImplementedException();
        }

        void IPersistStream.Load(IStream pstm)
        {
            throw new NotImplementedException();
        }

        void IPersistStream.Save(IStream pstm, int fClearDirty)
        {
            throw new NotImplementedException();
        }

        void IPersistStream.GetSizeMax(out _ULARGE_INTEGER pcbSize)
        {
            throw new NotImplementedException();
        }

        void IPersistStream.GetClassID(out Guid pClassID)
        {
            throw new NotImplementedException();
        }
    }
}