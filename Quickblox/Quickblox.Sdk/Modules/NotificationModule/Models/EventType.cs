namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    /// <summary>
    /// one_shot - a one-time event, which causes by an external object (the value is only valid if the 'date' is not specified)
    /// fixed_date - a one-time event, which occurs at a specified 'date' (the value is valid only if the 'date' is given)
    /// period_date - reusable event that occurs within a given 'period' from the initial 'date' (the value is only valid if the 'period' specified)
    /// </summary>
    public enum EventType
    {
        one_shot,
        fixed_date,
        period_date,
    }
}
