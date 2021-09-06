using ArchestrA.GRAccess;

namespace GServer.Archestra
{
    public class GraphicBuilder
    {
        private readonly IgObject _target;

        internal GraphicBuilder(IgObject target)
        {
            _target = target;
        }
    }
}