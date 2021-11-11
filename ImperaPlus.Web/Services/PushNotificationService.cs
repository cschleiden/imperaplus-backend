using System;
using ImperaPlus.Application;
using ImperaPlus.DTO.Notifications;
using ImperaPlus.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ImperaPlus.Web.Services
{
    public class GamePushNotificationService : IGameNotificationService
    {
        private IHubContext<GameHub> hubContext;

        public GamePushNotificationService(IHubContext<GameHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public void SendNotification(long gameId, Notification notification)
        {
            hubContext.Clients.Group(GameHub.GameGroup(gameId)).SendAsync("notification", notification);
        }

        public void SendNotification(long gameId, Guid teamId, Notification notification)
        {
            hubContext.Clients.Group(GameHub.GameTeamGroup(gameId, teamId)).SendAsync("notification", notification);
        }
    }

    public class UserPushNotificationService : IUserNotificationService
    {
        private IHubContext<GameHub> hubContext;

        public UserPushNotificationService(IHubContext<GameHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public void SendNotification(string userId, Notification notification)
        {
            hubContext.Clients.Group(userId).SendAsync("notification", notification);
        }
    }
}
