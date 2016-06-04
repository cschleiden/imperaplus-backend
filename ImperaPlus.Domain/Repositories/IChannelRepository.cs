using System.Collections.Generic;
using ImperaPlus.Domain.Chat;
using ImperaPlus.Domain.Enums;

namespace ImperaPlus.Domain.Repositories
{
    public interface IChannelRepository : IGenericRepository<Channel>
    {
        Channel GetByType(ChannelType general);
    }
}