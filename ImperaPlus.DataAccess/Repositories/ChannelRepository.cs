using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ImperaPlus.Domain.Chat;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.DataAccess.Repositories
{
    public class ChannelRepository : GenericRepository<Channel>, IChannelRepository
    {
        public ChannelRepository(DbContext context)
            : base(context)
        {
        }

        public Channel GetByType(ChannelType channelType)
        {
            return this.DbSet.Include(x => x.CreatedBy).Single(x => x.Type == channelType);
        }
    }
}