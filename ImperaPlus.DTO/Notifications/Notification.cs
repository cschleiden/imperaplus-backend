namespace ImperaPlus.DTO.Notifications
{
    /// <summary>
    /// Base type for all async notifications
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Initializes a new instance of Notification
        /// </summary>
        /// <param name="type"></param>
        public Notification(NotificationType type)
        {
            Type = type;
        }

        /// <summary>
        /// Type of notification
        /// </summary>
        public NotificationType Type { get; private set; }
    }
}
