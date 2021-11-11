using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using ImperaPlus.Application.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace ImperaPlus.Web.Hubs
{
    public interface INotificationHubContext
    {
    }

    [Authorize]
    public class GameHub : Hub, INotificationHubContext
    {
        public static string GameGroup(long gameId)
        {
            return string.Format(CultureInfo.InvariantCulture, "game-{0}", gameId);
        }

        public static string GameTeamGroup(long gameId, Guid teamId)
        {
            return string.Format(CultureInfo.InvariantCulture, "game-{0}-t-{1}", gameId, teamId);
        }

        private static readonly ConnectionMapping<string> Connections = new();

        private ILifetimeScope lifetimeScope;

        public GameHub(ILifetimeScope scope)
            : base()
        {
            lifetimeScope = scope;
        }

        public override async Task OnConnectedAsync()
        {
            // Track connection
            var userId = GetUserId();
            if (!Connections.GetConnections(userId).Contains(Context.ConnectionId))
            {
                Connections.Add(userId, Context.ConnectionId);
            }

            await AddGroup(userId);

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
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

            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Establish connections for the given game id
        /// </summary>
        /// <param name="gameId">Id of game to receive notifications for</param>
        public async Task JoinGame(long gameId)
        {
            var userId = GetUserId();

            var gameService = lifetimeScope.Resolve<IGameService>();
            var game = gameService.Get(gameId);
            var userTeam = game.Teams.FirstOrDefault(x => x.Players.Any(p => p.UserId == userId));
            if (userTeam == null)
            {
                throw new ArgumentException("gameId");
            }

            await AddGroup(GameGroup(gameId));
            await AddGroup(GameTeamGroup(gameId, userTeam.Id));
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

            var userId = GetUserId();

            var gameService = lifetimeScope.Resolve<IGameService>();
            var game = gameService.Get(gameId);
            var userTeam = game.Teams.FirstOrDefault(x => x.Players.Any(p => p.UserId == userId));
            if (userTeam == null)
            {
                throw new ArgumentException("gameId");
            }

            await LeaveGroup(GameGroup(gameId));
            await LeaveGroup(GameTeamGroup(gameId, userTeam.Id));
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
                await LeaveGame(oldGameId);
            }

            // Initialize for new game
            await JoinGame(newGameId);
        }

        /// <summary>
        /// Send message to given game. Message is relayed to other users in game chat.
        /// </summary>
        /// <param name="gameId">Id of game</param>
        /// <param name="text">Message text</param>
        /// <param name="isPublic">Value indicating whether message is inteded for all players or only team</param>
        public async Task SendGameMessage(long gameId, string text, bool isPublic)
        {
            var gameService = lifetimeScope.Resolve<IGameService>();
            var message = gameService.SendMessage(gameId, text, isPublic);

            // Relay to clients
            var groupName = isPublic ? GameGroup(gameId) : GameTeamGroup(gameId, message.TeamId);
            await Clients.Group(groupName).SendAsync("notification",
                new DTO.Notifications.GameChatMessageNotification { GameId = message.GameId, Message = message });
        }

        private async Task AddGroup(string groupName)
        {
            var userId = GetUserId();

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            Connections.JoinGroup(userId, groupName);
        }

        private async Task LeaveGroup(string groupName)
        {
            var userId = GetUserId();

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            Connections.LeaveGroup(userId, groupName);
        }

        private string GetUserId()
        {
            var userManager = lifetimeScope.Resolve<UserManager<Domain.User>>();
            return userManager.GetUserId(Context.User as ClaimsPrincipal);
        }
    }
}
