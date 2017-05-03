using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.DTO.Chat;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ApplicationException = ImperaPlus.Application.Exceptions.ApplicationException;

namespace ImperaPlus.Application.Chat
{
    public interface IChatService
    {
        /// <summary>
        /// Get channels for the given user
        /// </summary>
        /// <param name="getUserId">Id of user</param>
        /// <returns></returns>
        Task<IEnumerable<ChannelInformation>> GetChannelInformationForUser(string getUserId);

        void SendMessage(Guid channelId, string userId, string message);
    }

    public class ChatService : BaseService, IChatService
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public ChatService(IUnitOfWork unitOfWork, IUserProvider userProvider, RoleManager<IdentityRole> roleManager) 
            : base(unitOfWork, userProvider)
        {
            this.roleManager = roleManager;
        }

        public async Task<IEnumerable<ChannelInformation>> GetChannelInformationForUser(string userId)
        {
            var user = this.UnitOfWork.Users.FindByIdWithRoles(userId);

            if (user == null)
            {
                throw new ApplicationException("Cannot find user", ErrorCode.GenericApplicationError);
            }

            // Add default channels
            var channels = new List<ChannelInformation>
            {
                Mapper.Map<ChannelInformation>(
                    this.UnitOfWork.Channels.GetByType(ChannelType.General))
            };

            // Alliance channel if user is a member
            if (user.Alliance != null)
            {
                channels.Add(Mapper.Map<ChannelInformation>(user.Alliance.Channel));
            }            

            // Admin
            var adminRole = await this.roleManager.FindByNameAsync("admin");
            if (user.IsInRole(adminRole))
            {
                channels.Add(
                    Mapper.Map<ChannelInformation>(
                        this.UnitOfWork.Channels.GetByType(ChannelType.Admin)));
            }

            return channels;
        }

        public void SendMessage(Guid channelId, string userId, string message)
        {
            var channel = this.UnitOfWork.Channels.Query().FirstOrDefault(x => x.Id == channelId);
            if (null == channel)
            {
                // Channel does not exist in database.. might be transient, do not save anything
                return;
            }

            var user = this.UnitOfWork.Users.FindById(userId); 

            // TODO: CS: Check if user is allowed to post to channel!

            this.UnitOfWork.ChatMessages.Add(channel.CreateMessage(user, message));
            this.UnitOfWork.Commit();
        }
    }
}
