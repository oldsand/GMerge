using ArchestrA.GRAccess;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace GalaxyMerge.Archestra
{
    public class GalaxyCommandResult
    {
        public GalaxyCommandResult(ICommandResult commandResult)
        {
            Successful = commandResult.Successful;
            Text = commandResult.Text;
            ResultType = commandResult.ID.ToString();
            Message = commandResult.CustomMessage;
        }
        
        public bool Successful { get; private set; }
        public string Text { get; private set; }
        public string ResultType { get; private set; }
        public string Message { get; private set; }
    }
}