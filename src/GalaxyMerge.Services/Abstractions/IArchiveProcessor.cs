using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Services.Abstractions
{
    public interface IArchiveProcessor
    {
        void Enqueue(ChangeLog changeLog);
    }
}