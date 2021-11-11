using System;

namespace ImperaPlus.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAllianceRepository Alliances { get; }

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
    }
}
