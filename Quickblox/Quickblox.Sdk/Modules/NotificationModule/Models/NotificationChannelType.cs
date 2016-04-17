namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    /// <summary>
    /// Declare which notification channels could be used to notify user about events. Allowed values: email, apns, gcm, mpns, bbps.
    /// </summary>
    public enum NotificationChannelType
    {
        apns,
        gcm,
        mpns,
        bbps,
        email
    }
}