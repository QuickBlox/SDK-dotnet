using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;

namespace QMunicate.Helper
{
    public interface IPushNotificationsManager
    {
        Task UpdatePushTokenIfNeeded(PushNotificationChannel pushChannel);
        Task DeletePushToken();
        Task<bool> CreateSubscriptionIfNeeded();
        Task DeleteSubscription();
    }
}
