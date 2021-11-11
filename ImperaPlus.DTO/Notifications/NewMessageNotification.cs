namespace ImperaPlus.DTO.Notifications
{
    public class NewMessageNotification : Notification
    {
        public NewMessageNotification() : base(NotificationType.NewMessage)
        {
        }

        /// <summary>
        /// Name of sender
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// Subject of new message
        /// </summary>
        public string Subject { get; set; }
    }
}
