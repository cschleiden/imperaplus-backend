using System;
using ImperaPlus.DataAccess.Repositories;
using ImperaPlus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ImperaContext context;

        private IGameRepository games;
        private IMapTemplateDescriptorRepository mapTemplates;
        private IUserRepository userRepository;
        private IChannelRepository channels;
        private IChatMessageRepository chatMessages;
        private IRoleRepository roleRepository;
        private INewsRepository newsRepository;
        private IPlayerRepository playerRepository;
        private ITeamRepository teamRepository;
        private ILadderRepository ladderRepository;
        private IMessageRepository messageRepository;
        private ITournamentRepository tournamentRepository;
        private IAllianceRepository allianceRepository;

        public UnitOfWork(ImperaContext context)
        {
            this.context = context;
        }

        public ImperaContext Context => context;

        public IGameRepository Games => games ?? (games = new GameRepository(context));

        public IChannelRepository Channels => channels ?? (channels = new ChannelRepository(context));

        public IChatMessageRepository ChatMessages =>
            chatMessages ?? (chatMessages = new ChatMessageRepository(context));

        public IMapTemplateDescriptorRepository MapTemplateDescriptors =>
            mapTemplates ?? (mapTemplates = new MapTemplateDescriptorRepository(context));

        public IUserRepository Users => userRepository ?? (userRepository = new UserRepository(context));

        public IRoleRepository Roles => roleRepository ?? (roleRepository = new RoleRepository(context));

        public INewsRepository News => newsRepository ?? (newsRepository = new NewsRepository(context));

        public IPlayerRepository Players => playerRepository ?? (playerRepository = new PlayerRepository(context));

        public ITeamRepository Teams => teamRepository ?? (teamRepository = new TeamRepository(context));

        public ILadderRepository Ladders => ladderRepository ?? (ladderRepository = new LadderRepository(context));

        public IMessageRepository Messages => messageRepository ?? (messageRepository = new MessageRepository(context));

        public ITournamentRepository Tournaments =>
            tournamentRepository ?? (tournamentRepository = new TournamentRepository(context));

        public IAllianceRepository Alliances =>
            allianceRepository ?? (allianceRepository = new AllianceRepository(context));

        public void Commit()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            catch (Exception)
            {
                // TODO: CS: Handle errors
                throw;
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }


        public IGenericRepository<T> GetGenericRepository<T>() where T : class
        {
            return new GenericRepository<T>(context);
        }
    }
}
