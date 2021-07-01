using System;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Core.Naming;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Core.Prism
{
    public static class DialogServiceExtension
    {
        public static void ShowConfirmation(this IDialogService dialogService, string title, string message,
            Action<IDialogResult> callBack)
        {
            var parameters = new DialogParameters
            {
                {"title", title},
                {"message", message}
            };
            
            dialogService.ShowDialog(DialogName.ConfirmationDialog, parameters, callBack);
        }

        public static void ShowNotification(this IDialogService dialogService, string title, string message,
            Action<IDialogResult> callBack)
        {
            var parameters = new DialogParameters
            {
                {"title", title},
                {"message", message}
            };
            
            dialogService.ShowDialog(DialogName.ConfirmationDialog, parameters, callBack);
        }

        public static void ShowError(this IDialogService dialogService, string title, string message, Exception ex,
            Action<IDialogResult> callBack)
        {
            var parameters = new DialogParameters
            {
                {"title", title},
                {"message", message},
                {"exception", ex}
            };

            dialogService.ShowDialog(DialogName.ErrorDialog, parameters, callBack);
        }
    }
}