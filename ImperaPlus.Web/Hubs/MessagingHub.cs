using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using ImperaPlus.Application.Chat;
using ImperaPlus.DTO.Chat;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.Identity;

namespace ImperaPlus.Web.Hubs
{
    [HubName("chat")]
    [Authorize]
    public class MessagingHub : Hub
    {
        private readonly IChatService chatService;

        private readonly static ConnectionMapping<string> Connections =
            new ConnectionMapping<string>();
        private UserManager<Domain.User> userManager;

        public MessagingHub(ILifetimeScope scope, UserManager<Domain.User> userManager)
            : base()
        {
            var lifetimeScope = scope.BeginLifetimeScope("AutofacWebRequest");
            this.chatService = lifetimeScope.Resolve<IChatService>();
            this.userManager = userManager;
        }

        public override Task OnConnected()
        {
            // Track connection            
            string userName = this.userManager.GetUserName(ClaimsPrincipal.Current);
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
            string userName = this.userManager.GetUserName(ClaimsPrincipal.Current);

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
            string userId = this.userManager.GetUserId(this.Context.User as ClaimsPrincipal);
            var user = this.userManager.GetUserAsync(this.Context.User as ClaimsPrincipal).Result;
            string userName = user.UserName;

            // Add users to appropriate groups
            var channels = this.chatService.GetChannelInformationForUser(userId).Result;
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
            string userId = this.userManager.GetUserId(this.Context.User as ClaimsPrincipal);
            this.chatService.SendMessage(channelId, userId, message);

            // Send message to currently online players
            this.Clients.Group(channelId.ToString()).broadcastMessage(new Message
            {
                ChannelIdentifier = channelId.ToString(),
                UserName = this.Context.User.Identity.Name,
                DateTime = DateTime.UtcNow,
                Text = message
            });
        }
    }
}