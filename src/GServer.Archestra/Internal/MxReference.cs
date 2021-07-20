using System;
using System.Runtime.CompilerServices;
using ArchestrA.GRAccess;

[assembly: InternalsVisibleTo("GServer.Archestra.UnitTests")]
[assembly: InternalsVisibleTo("GServer.Archestra.UnitTests.ExtensionTests")]

namespace GServer.Archestra.Internal
{
    
    /// <summary>
    /// Since Archestra doesn't give us a way to create a reference type, I am implementing the interface just to set
    /// the public properties and pass into MxValue Get/Set extensions. Class only used for transferring data
    /// </summary>
    internal class MxReference : IMxReference
    {
        void IPersist.GetClassID(out Guid pClassID)
        {
            throw new NotImplementedException();
        }

        void IMxReference.IsDirty()
        {
            throw new NotImplementedException();
        }

        void IMxReference.Load(IStream pstm)
        {
            throw new NotImplementedException();
        }

        void IMxReference.Save(IStream pstm, int fClearDirty)
        {
            throw new NotImplementedException();
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