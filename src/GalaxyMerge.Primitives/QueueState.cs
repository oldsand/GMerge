using GalaxyMerge.Core;

namespace GalaxyMerge.Primitives
{
    public class QueueState : Enumeration
    {
        private QueueState()
        {
        }

        private QueueState(int id, string name) : base(id, name)
        {
        }

        public static readonly QueueState New = new QueueState(0, "New");
        public static readonly QueueState Processing = new QueueState(1, "Processing");
        public static readonly QueueState Failed = new QueueState(2, "Failed");
    }
}