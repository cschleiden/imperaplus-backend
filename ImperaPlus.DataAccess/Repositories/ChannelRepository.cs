using System;
using System.Linq;
using ImperaPlus.Domain.Chat;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.DataAccess.Repositories
{
    public class ChannelRepository : GenericRepository<Channel>, IChannelRepository
    {
        public ChannelRepository(DbContext context)
            : base(context)
        {
        }

        public Channel GetById(Guid channelId)
        {
            var channel = DbSet.First(x => x.Id == channelId);
            AddMessages(channel);
            return channel;
        }

        public Channel GetByType(ChannelType channelType)
        {
            var channel = DbSet.First(x => x.Type == channelType);
            AddMessages(channel);
            return channel;
        }

        private void AddMessages(Channel channel)
        {
            channel.RecentMessages = Context
                .Entry(channel)
                .Collection(c => c.Messages)
                .Query()
                .Include(x => x.CreatedBy)
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.CreatedBy != null && !x.CreatedBy.IsDeleted)
                .Take(20)
                .OrderBy(x => x.CreatedAt);
        }
    }
}
