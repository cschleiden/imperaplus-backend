using System;
using System.Collections.Generic;

namespace ImperaPlus.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGameRepository Games { get; }

        ITeamRepository Teams { get; }

        IPlayerRepository Players { get; }

        IMapTemplateDescriptorRepository MapTemplateDescriptors { get; }

        IChannelRepository Channels { get; }

        IChatMessageRepository ChatMessages { get; }

        IUserRepository Users { get; }

        IRoleRepository Roles { get; }

        INewsRepository News { get; }

        ILadderRepository Ladders { get; }

        IMessageRepository Messages { get; }

        ITournamentRepository Tournaments { get; }

        IGenericRepository<T> GetGenericRepository<T>() where T : class;

        void Commit();

        IEnumerable<T> GetChangedEntities<T>() where T : class;
    }
}