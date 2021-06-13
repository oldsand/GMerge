using System;
using GalaxyMerge.Client.Core.Mvvm;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Core.Prism
{
    public static class DialogServiceExtension
    {
        public static void ShowConfirmation(this IDialogService dialogService, string message, Action<IDialogResult> callBack)
        {
            dialogService.ShowDialog(DialogName.ConfirmationDialog, new DialogParameters($"message={message}"),
                callBack);
        }

        public static void ShowNotification(this IDialogService dialogService, string message,
            Action<IDialogResult> callBack)
        {
            dialogService.ShowDialog("NotificationDialog", new DialogParameters($"message={message}"), callBack,
                "notificationWindow");
        }
    }
}