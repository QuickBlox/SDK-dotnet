namespace Quickblox.Sdk.Modules.MessagesModule.Models
{
    /// <summary>
    /// Wrap xmmpp subscriptions state
    /// </summary>
    public enum SubscriptionState
    {
        /// <summary>
        /// The user does not have a subscription to the contact's presence information,
        /// and the contact does not have a subscription to the user's presence
        /// information.
        /// </summary>
        None,
        /// <summary>
        /// The user has a subscription to the contact's presence information, but the
        /// contact does not have a subscription to the user's presence information.
        /// </summary>
        To,
        /// <summary>
        /// The contact has a subscription to the user's presence information, but the
        /// user does not have a subscription to the contact's presence information.
        /// </summary>
        From,
        /// <summary>
        /// Both the user and the contact have subscriptions to each other's presence
        /// information.
        /// </summary>
        Both
    }
}
