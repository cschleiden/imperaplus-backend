using System;
using ImperaPlus.Domain.Chat;
using ImperaPlus.Domain.Enums;

namespace ImperaPlus.Domain.Repositories
{
    public interface IChannelRepository : IGenericRepository<Channel>
    {
        Channel GetById(Guid channelId);

        Channel GetByType(ChannelType general);
    }
}
