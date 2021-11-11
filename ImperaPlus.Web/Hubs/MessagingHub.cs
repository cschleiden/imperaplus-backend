using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using ImperaPlus.Application.Chat;
using ImperaPlus.DTO.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ImperaPlus.Web.Hubs
{
    public interface IMessagingHubContext
    {
    }

    [Authorize]
    public class MessagingHub : Hub, IMessagingHubContext
    {
        private static readonly ConnectionMapping<string> Connections = new();

        private ILogger<MessagingHub> logger;

        private ILifetimeScope lifetimeScope;

        public MessagingHub(
            ILifetimeScope lifetimeScope,
            ILogger<MessagingHub> logger)
            : base()
        {
            this.lifetimeScope = lifetimeScope;
            this.logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            // Track connection
            var userName = GetUser().UserName;
            Connections.Add(userName, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string name;
            IEnumerable<string> channels;
            if (Connections.Remove(Context.ConnectionId, out channels, out name))
            {
                // Remove clients from groups
                foreach (var channel in channels)
                {
                    await Clients.OthersInGroup(channel).SendAsync("leave",
                        new UserChangeEvent { ChannelIdentifier = channel, UserName = name });
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Initialize connection to the chat and notify server that client is ready to receive messages
        /// </summary>
        /// <returns></returns>
        public async Task<ChatInformation> Init()
        {
            var userId = GetUserId();
            var userName = GetUser().UserName;

            // Add users to appropriate groups
            var chatService = lifetimeScope.Resolve<IChatService>();
            var channels = chatService.GetChannelInformationForUser(userId).Result;
            foreach (var channel in channels)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, channel.Identifier);
                Connections.JoinGroup(userName, channel.Identifier);

                // Inform other clients in channel/group
                await Clients.OthersInGroup(channel.Identifier).SendAsync("join",
                    new UserChangeEvent { ChannelIdentifier = channel.Identifier, UserName = userName });

                // Add information about current users in channels
                channel.Users =
                    Connections.GetUsersForGroup(channel.Identifier)
                        .Select(x => new User { Type = UserType.None, Name = x }).ToArray();
            }

            return new ChatInformation() { Channels = channels.ToArray() };
        }

        public void SendMessage(Guid channelId, string message)
        {
            // Send to service for persistence
            var chatService = lifetimeScope.Resolve<IChatService>();
            var userId = GetUserId();
            chatService.SendMessage(channelId, userId, message);

            // Send message to currently online players
            Clients.Group(channelId.ToString()).SendAsync("broadcastMessage",
                new ChatMessage
                {
                    ChannelIdentifier = channelId.ToString(),
                    UserName = GetUser().UserName,
                    DateTime = DateTime.UtcNow,
                    Text = message
                });
        }

        private string GetUserId()
        {
            var userManager = lifetimeScope.Resolve<UserManager<Domain.User>>();
            return userManager.GetUserId(Context.User as ClaimsPrincipal);
        }

        private Domain.User GetUser()
        {
            var userManager = lifetimeScope.Resolve<UserManager<Domain.User>>();
            return userManager.GetUserAsync(Context.User as ClaimsPrincipal).Result;
        }
    }
}
