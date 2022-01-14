using System;
using GClient.Core.Mvvm;
using GClient.Core.Naming;
using Prism.Services.Dialogs;

namespace GClient.Core.Prism
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