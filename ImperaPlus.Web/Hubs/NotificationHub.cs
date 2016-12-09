using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using ImperaPlus.Application.Games;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.DTO.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;

namespace ImperaPlus.Web.Hubs
{
    public interface INotificationHubContext
    {
        void Notification(Notification notification);
    }

    [HubName("notification")]
    [Authorize]
    public class NotificationHub : Hub
    {
        public static string GameGroup(long gameId)
        {
            return string.Format(CultureInfo.InvariantCulture, "game-{0}", gameId);
        }

        public static string GameTeamGroup(long gameId, Guid teamId)
        {
            return string.Format(CultureInfo.InvariantCulture, "game-{0}-t-{1}", gameId, teamId);
        }

        private readonly static ConnectionMapping<string> Connections = new ConnectionMapping<string>();

        private readonly IGameService gameService;
        private readonly IUnitOfWork unitOfWork;
        private UserManager<User> userManager;

        public NotificationHub(ILifetimeScope scope)
            : base()
        {
            var lifetimeScope = scope.BeginLifetimeScope("AutofacWebRequest");

            this.gameService = lifetimeScope.Resolve<IGameService>();
            this.unitOfWork = lifetimeScope.Resolve<IUnitOfWork>();
            this.userManager = lifetimeScope.Resolve<UserManager<User>>();
        }

        public override async Task OnConnected()
        {
            // Track connection
            string userId = this.userManager.GetUserId(ClaimsPrincipal.Current);
            if (!Connections.GetConnections(userId).Contains(Context.ConnectionId))
            {
                Connections.Add(userId, Context.ConnectionId);
            }

            await this.AddGroup(userId);

            await base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string userId;
            IEnumerable<string> channels;
            if (Connections.Remove(Context.ConnectionId, out channels, out userId))
            {
                // Remove clients from groups
                foreach (var channel in channels)
                {
                    // TODO: CS: Do anything here?
                    // this.Clients.OthersInGroup(channel).leave(new UserChangeEvent
                    // {
                    //     ChannelIdentifier = channel,
                    //     UserName = name
                    // });
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string userId = this.userManager.GetUserId(ClaimsPrincipal.Current);
            if (!Connections.GetConnections(userId).Contains(Context.ConnectionId))
            {
                Connections.Add(userId, Context.ConnectionId);
            }

            return base.OnReconnected();
        }

        /// <summary>
        /// Establish connections for the given game id
        /// </summary>
        /// <param name="gameId">Id of game to receive notifications for</param>
        public async Task JoinGame(long gameId)
        {
            string userId = this.userManager.GetUserId(ClaimsPrincipal.Current);
            var game = this.gameService.Get(gameId);
            var userTeam = game.Teams.FirstOrDefault(x => x.Players.Any(p => p.UserId == userId));
            if (userTeam == null)
            {
                throw new ArgumentException("gameId");
            }

            await this.AddGroup(GameGroup(gameId));
            await this.AddGroup(GameTeamGroup(gameId, userTeam.Id));
        }

        /// <summary>
        /// End receiving game notifications
        /// </summary>
        /// <param name="gameId">Id of game</param>
        public async Task LeaveGame(long gameId)
        {
            if (gameId == 0)
            {
                return;
            }

            string userId = this.userManager.GetUserId(ClaimsPrincipal.Current);

            var game = this.gameService.Get(gameId);
            var userTeam = game.Teams.FirstOrDefault(x => x.Players.Any(p => p.UserId == userId));
            if (userTeam == null)
            {
                throw new ArgumentException("gameId");
            }

            await this.LeaveGroup(GameGroup(gameId));
            await this.LeaveGroup(GameTeamGroup(gameId, userTeam.Id));
        }

        /// <summary>
        /// Switch game context
        /// </summary>
        /// <param name="oldGameId">Id of old game</param>
        /// <param name="newGameId">Id of new game</param>
        public async Task SwitchGame(long oldGameId, long newGameId)
        {
            if (oldGameId > 0)
            {
                // Leave current groups
                await this.LeaveGame(oldGameId);
            }

            // Initialize for new game
            await this.JoinGame(newGameId);
        } 

        /// <summary>
        /// Send message to given game. Message is relayed to other users in game chat.
        /// </summary>
        /// <param name="gameId">Id of game</param>
        /// <param name="text">Message text</param>
        /// <param name="isPublic">Value indicating whether message is inteded for all players or only team</param>
        public async Task SendGameMessage(long gameId, string text, bool isPublic)
        {
            string userId = this.userManager.GetUserId(ClaimsPrincipal.Current);

            var message = this.gameService.SendMessage(gameId, text, isPublic);

            // Relay to clients
            var groupName = isPublic ? GameGroup(gameId) : GameTeamGroup(gameId, message.TeamId);
            await this.Clients.Group(groupName).notification(new ImperaPlus.DTO.Notifications.GameChatMessageNotification
            {
                GameId = message.GameId,
                Message = message
            });
        }

        private async Task AddGroup(string groupName)
        {
            string userId = this.userManager.GetUserId(ClaimsPrincipal.Current);

            await this.Groups.Add(this.Context.ConnectionId, groupName);
            Connections.JoinGroup(userId, groupName);
        }

        private async Task LeaveGroup(string groupName)
        {
            string userId = this.userManager.GetUserId(ClaimsPrincipal.Current);

            await this.Groups.Remove(this.Context.ConnectionId, groupName);
            Connections.LeaveGroup(userId, groupName);
        }
    }
}