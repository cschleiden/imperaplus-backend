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

        public Channel FindById(Guid id)
        {
            return this.DbSet.FirstOrDefault(x => x.Id == id);
        }

        public Channel GetByType(ChannelType channelType)
        {
            return this.DbSet.Include(x => x.CreatedBy).Single(x => x.Type == channelType);
        }
    }
}