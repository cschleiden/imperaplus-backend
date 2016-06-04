using System;
using ImperaPlus.Application;
using ImperaPlus.Backend.Hubs;
using ImperaPlus.DTO.Notifications;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace ImperaPlus.Backend.Services
{
    public class GamePushNotificationService : IGameNotificationService
    {
        private IHubContext<INotificationHubContext> hubContext;

        public GamePushNotificationService(IHubContext<INotificationHubContext> hubContext)
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
        private IHubContext<INotificationHubContext> hubContext;

        public UserPushNotificationService(IHubContext<INotificationHubContext> hubContext)
        {
            this.hubContext = hubContext;
        }

        public void SendNotification(string userId, Notification notification)
        {
            this.hubContext.Clients.Group(userId).Notification(notification);
        }
    }
}