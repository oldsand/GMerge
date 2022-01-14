using System;
using ArchestrA.GRAccess;
using GServer.Archestra.Exceptions;

namespace GServer.Archestra.Extensions
{
    public static class CommandResultExtensions
    {
        public static void Process(this ICommandResult commandResult)
        {
            if (commandResult.Successful) return;

            switch (commandResult.ID)
            {
                case EGRCommandResult.cmdUnknownError:
                    throw new GalaxyException(commandResult.CustomMessage);
                case EGRCommandResult.cmdSuccess:
                    break;
                case EGRCommandResult.cmdInsufficientPermissions:
                    throw new InsufficientPermissionsException(commandResult.CustomMessage);
                case EGRCommandResult.cmdNoSuchGRNode:
                    throw new NoSuchGrNodeException(commandResult.CustomMessage);
                case EGRCommandResult.cmdNoSuchUser:
                    throw new NoSuchUserException(commandResult.CustomMessage);
                case EGRCommandResult.cmdPasswordIncorrect:
                    throw new PasswordIncorrectException(commandResult.CustomMessage);
                case EGRCommandResult.cmdLicenseUnavailable:
                    throw new LicenseUnavailableException(commandResult.CustomMessage);
                case EGRCommandResult.cmdNoSuchFile:
                    throw new NoSuchFileException(commandResult.CustomMessage);
                case EGRCommandResult.cmdCouldntCreateFile:
                    throw new CouldNotCreateFileException(commandResult.CustomMessage);
                case EGRCommandResult.cmdObjectIsCheckedOut:
                    throw new ObjectIsCheckedOutException(commandResult.CustomMessage);
                case EGRCommandResult.cmdObjectIsCheckedOutToSomeoneElse:
                    throw new ObjectIsCheckedOutToSomeoneElseException(commandResult.CustomMessage);
                case EGRCommandResult.cmdObjectNotCheckedOutToMe:
                    throw new ObjectNotCheckedOutToMeException(commandResult.CustomMessage);
                case EGRCommandResult.cmdObjectCannotBeOverwritten:
                    throw new ObjectCannotBeOverwrittenException(commandResult.CustomMessage);
                case EGRCommandResult.cmdTemplateInUse:
                    throw new TemplateInUseException(commandResult.CustomMessage);
                case EGRCommandResult.cmdObjectIsAContainer:
                    throw new ObjectIsAContainerException(commandResult.CustomMessage);
                case EGRCommandResult.cmdObjectHostNotFound:
                    throw new ObjectHostNotFoundException(commandResult.CustomMessage);
                case EGRCommandResult.cmdInstanceIsHost:
                    throw new InstanceIsHostException(commandResult.CustomMessage);
                case EGRCommandResult.cmdObjectIsRequired:
                    throw new ObjectIsRequiredException(commandResult.CustomMessage);
                case EGRCommandResult.cmdObjectNotAnAutomationObject:
                    throw new ObjectNotAnAutomationObjectException(commandResult.CustomMessage);
                case EGRCommandResult.cmdObjectInBadState:
                    throw new ObjectInBadStateException(commandResult.CustomMessage);
                case EGRCommandResult.cmdCustomConfigurationError:
                    throw new CustomConfigurationErrorException(commandResult.CustomMessage);
                case EGRCommandResult.cmdObjectInReadOnlyMode:
                    throw new ObjectInReadOnlyModeException(commandResult.CustomMessage);
                case EGRCommandResult.cmdObjectHostNotDeployed:
                    throw new ObjectHostNotDeployedException(commandResult.CustomMessage);
                case EGRCommandResult.cmdInstanceIsDeployed:
                    throw new InstanceIsDeployedException(commandResult.CustomMessage);
                case EGRCommandResult.cmdInvallidGRLoadMode:
                    throw new InvalidGrLoadModeException(commandResult.CustomMessage);
                case EGRCommandResult.cmdOEMVersionIncompatible:
                    throw new OemVersionIncompatibleException(commandResult.CustomMessage);
                case EGRCommandResult.cmdRetryClientSync:
                    throw new RetryClientSyncException(commandResult.CustomMessage);
                case EGRCommandResult.cmdSyncSemaphore:
                    throw new SyncSemaphoreException(commandResult.CustomMessage);
                case EGRCommandResult.cmdOutOfSync:
                    throw new OutOfSyncException(commandResult.CustomMessage);
                case EGRCommandResult.cmdInstanceProtectionDenied:
                    throw new InstanceProtectionDeniedException(commandResult.CustomMessage);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void Process(this ICommandResults commandResults)
        {
            if (commandResults.CompletelySuccessful) return;

            foreach (ICommandResult commandResult in commandResults)
                commandResult.Process();
        }
    }
}