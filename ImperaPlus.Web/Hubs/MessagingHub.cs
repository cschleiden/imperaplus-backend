using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using ImperaPlus.Application.Chat;
using ImperaPlus.DTO.Chat;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;
using Microsoft.Extensions.Logging;

namespace ImperaPlus.Web.Hubs
{
    public interface IMessagingHubContext : IHub
    {
    }

    [HubName("chat")]
    [Authorize]
    public class MessagingHub : Hub, IMessagingHubContext
    {
        private readonly static ConnectionMapping<string> Connections =
            new ConnectionMapping<string>();
        
        private ILogger<MessagingHub> logger;

        private ILifetimeScope lifetimeScope;

        public MessagingHub(
            ILifetimeScope lifetimeScope,
            ILogger<MessagingHub> logger)
            : base()
        {
            this.lifetimeScope = lifetimeScope.BeginLifetimeScope();
            this.logger = logger;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.lifetimeScope != null)
            {
                this.lifetimeScope.Dispose();
                this.lifetimeScope = null;
            }

            base.Dispose(disposing);
        }

        public override Task OnConnected()
        {
            // Track connection
            string userName = this.GetUser().UserName;
            Connections.Add(userName, this.Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name;
            IEnumerable<string> channels;
            if (Connections.Remove(Context.ConnectionId, out channels, out name))
            {
                // Remove clients from groups
                foreach (var channel in channels)
                {
                    this.Clients.OthersInGroup(channel).leave(new UserChangeEvent
                    {
                        ChannelIdentifier = channel,
                        UserName = name
                    });
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string userName = this.GetUser().UserName;
            if (!Connections.GetConnections(userName).Contains(Context.ConnectionId))
            {
                Connections.Add(userName, Context.ConnectionId);
            }

            return base.OnReconnected();
        }

        /// <summary>
        /// Initialize connection to the chat and notify server that client is ready to receive messages
        /// </summary>
        /// <returns></returns>
        public ChatInformation Init()
        {
            string userId = this.GetUserId();
            string userName = this.GetUser().UserName;

            // Add users to appropriate groups
            var chatService = this.lifetimeScope.Resolve<IChatService>();
            var channels = chatService.GetChannelInformationForUser(userId).Result;
            foreach (var channel in channels)
            {
                this.Groups.Add(this.Context.ConnectionId, channel.Identifier);
                Connections.JoinGroup(userName, channel.Identifier);

                // Inform other clients in channel/group
                this.Clients.OthersInGroup(channel.Identifier).@join(new UserChangeEvent
                {
                    ChannelIdentifier = channel.Identifier,
                    UserName = userName
                });

                // Add information about current users in channels
                channel.Users =
                    Connections.GetUsersForGroup(channel.Identifier)
                        .Select(x => new User
                        {
                            Type = UserType.None,
                            Name = x
                        }).ToArray();
            }
            
            return new ChatInformation()
            {
                Channels = channels.ToArray()
            };
        }

        public void SendMessage(Guid channelId, string message)
        {
            // Send to service for persistence            
            var chatService = this.lifetimeScope.Resolve<IChatService>();
            string userId = this.GetUserId();
            chatService.SendMessage(channelId, userId, message);

            // Send message to currently online players
            var user = this.GetUserId();
            this.Clients.Group(channelId.ToString()).broadcastMessage(new ChatMessage
            {
                ChannelIdentifier = channelId.ToString(),
                UserName = this.GetUser().UserName,
                DateTime = DateTime.UtcNow,
                Text = message
            });
        }

        private string GetUserId()
        {
            var userManager = this.lifetimeScope.Resolve<UserManager<Domain.User>>();
            return userManager.GetUserId(this.Context.User as ClaimsPrincipal);
        }

        private Domain.User GetUser()
        {
            var userManager = this.lifetimeScope.Resolve<UserManager<Domain.User>>();
            return userManager.GetUserAsync(this.Context.User as ClaimsPrincipal).Result;
        }
    }
}