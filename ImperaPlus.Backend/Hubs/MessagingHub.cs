using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImperaPlus.Application.Chat;
using ImperaPlus.DTO.Chat;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Autofac;
using System.Globalization;

namespace ImperaPlus.Backend.Hubs
{   
    [HubName("chat")]
    [Authorize]
    public class MessagingHub : Hub
    {
        private readonly IChatService chatService;

        private readonly static ConnectionMapping<string> Connections =
            new ConnectionMapping<string>();

        public MessagingHub(ILifetimeScope scope)
            : base()
        {
            var lifetimeScope = scope.BeginLifetimeScope("AutofacWebRequest");
            this.chatService = lifetimeScope.Resolve<IChatService>();
        }

        public override Task OnConnected()
        {
            // Track connection
            Connections.Add(this.Context.User.Identity.GetUserName(), this.Context.ConnectionId);            

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
            string userName = Context.User.Identity.GetUserName();

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
            var userId = this.Context.User.Identity.GetUserId();
            var userName = this.Context.User.Identity.GetUserName();

            // Add users to appropriate groups
            var channels = this.chatService.GetChannelInformationForUser(userId);

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
            this.chatService.SendMessage(channelId, this.Context.User.Identity.GetUserId(), message);

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