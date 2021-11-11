using Hangfire;
using ImperaPlus.Application.Jobs;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Games.Events;

namespace ImperaPlus.Application.Notifications
{
    /// <summary>
    /// Handlers for push notifications
    /// </summary>
    public class PushNotifications
        : ICompletedEventHandler<TurnEndedEvent>, ICompletedEventHandler<PlayerSurrenderedEvent>
    {
        private IBackgroundJobClient backgroundJobClient;
        private IGameNotificationService gameNotificationService;

        public PushNotifications(IBackgroundJobClient backgroundJobClient,
            IGameNotificationService gameNotificationService)
        {
            this.backgroundJobClient = backgroundJobClient;
            this.gameNotificationService = gameNotificationService;
        }

        public void Handle(TurnEndedEvent evt)
        {
            // Notify user whose turn it's now
            var userId = evt.Game.CurrentPlayer.UserId;
            var notification = new DTO.Notifications.PlayersTurnNotification { GameId = evt.Game.Id };
            backgroundJobClient.Enqueue<NotificationJob>(p => p.SendToUser(userId, notification));

            // Notify all active users in game
            var gameId = evt.Game.Id;
            var turnEndedNotification = new DTO.Notifications.TurnEndedNotification
            {
                GameId = evt.Game.Id, NewPlayerId = evt.Game.CurrentPlayerId.GetValueOrDefault()
            };

            // Send in current request
            gameNotificationService.SendNotification(gameId, turnEndedNotification);
        }

        public void Handle(PlayerSurrenderedEvent evt)
        {
            // Notify all active users in game
            var gameId = evt.Game.Id;
            DTO.Notifications.Notification notification =
                new DTO.Notifications.PlayerSurrenderedNotification { GameId = evt.Game.Id };

            // Send in current request
            gameNotificationService.SendNotification(gameId, notification);
        }
    }
}
