using Autofac;
using Hangfire;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Normal)]
    [AutomaticRetry(Attempts = 0)]
    public class NotificationJob : Job
    {
        public NotificationJob(ILifetimeScope scope)
            : base(scope)
        {            
        }

        public void SendToUser(string userId, DTO.Notifications.Notification notification)
        {
            var pushNotificationService = this.LifetimeScope.Resolve<IUserNotificationService>();

            pushNotificationService.SendNotification(userId, notification);
        }

        public void SendToGame(long gameId, DTO.Notifications.Notification notification)
        {
            var pushNotificationService = this.LifetimeScope.Resolve<IGameNotificationService>();

            pushNotificationService.SendNotification(gameId, notification);
        }
    }
}
