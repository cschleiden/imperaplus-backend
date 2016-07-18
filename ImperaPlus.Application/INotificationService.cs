namespace ImperaPlus.Application
{
    public interface IUserNotificationService
    {
        /// <summary>
        /// Send notification to given player
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="notification"></param>
        void SendNotification(string userId, DTO.Notifications.Notification notification);
    }

    public interface IGameNotificationService
    {

        /// <summary>
        /// Send notification to given game
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="notification"></param>
        void SendNotification(long gameId, DTO.Notifications.Notification notification);
    }
}
