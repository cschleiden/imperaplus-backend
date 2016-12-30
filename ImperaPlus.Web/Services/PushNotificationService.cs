using System;
using ImperaPlus.Application;
using ImperaPlus.DTO.Notifications;
using ImperaPlus.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ImperaPlus.Web.Services
{
    public class GamePushNotificationService : IGameNotificationService
    {
        private IHubContext<NotificationHub> hubContext;

        public GamePushNotificationService(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public void SendNotification(long gameId, Notification notification)
        {
            this.hubContext.Clients.Group(NotificationHub.GameGroup(gameId)).Notification(notification);
        }

        public void SendNotification(long gameId, Guid teamId, Notification notification)
        {
            this.hubContext.Clients.Group(NotificationHub.GameTeamGroup(gameId, teamId)).Notification(notification);
        }
    }

    public class UserPushNotificationService : IUserNotificationService
    {
        private IHubContext<NotificationHub> hubContext;

        public UserPushNotificationService(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public void SendNotification(string userId, Notification notification)
        {
            this.hubContext.Clients.Group(userId).Notification(notification);
        }
    }
}