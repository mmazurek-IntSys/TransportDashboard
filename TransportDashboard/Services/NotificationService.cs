using ToastNotifications;
using ToastNotifications.Position;
using ToastNotifications.Lifetime;
//using ToastNotifications.Messages;
using Microsoft.Toolkit.Uwp.Notifications;
namespace TransportDashboard.Services;

public static class NotificationService
{
    public static void ShowToast(
        string title,
        string message)
    {
        new ToastContentBuilder()
            .AddText(title)
            .AddText(message)
            .Show();
    }
}