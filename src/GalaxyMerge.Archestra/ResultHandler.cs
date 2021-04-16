using System.Collections.Generic;
using ArchestrA.GRAccess;

namespace GalaxyMerge.Archestra
{
    public static class ResultHandler
    {
        public static void Handle(ICommandResult result, string name)
        {
            var operationResult = new GalaxyCommandResult(result);
            if (operationResult.Successful) return;

            throw new GalaxyException(
                $"Command failed for '{name}' with. Message: {result.CustomMessage}. See Results property for more details.",
                operationResult);
        }

        public static void Handle(ICommandResults results, string name)
        {
            if (results.CompletelySuccessful) return;

            var operationResults = new List<GalaxyCommandResult>();
            
            foreach (ICommandResult result in results)
            {
                var operationResult = new GalaxyCommandResult(result);
                if (!operationResult.Successful)
                    operationResults.Add(operationResult);
            }
            
            throw new GalaxyException(
                $"One or more commands failed for '{name}'. See Results property for more details.", operationResults);
        }
    }
}