using System;
using System.Linq;
using ImperaPlus.DataAccess.Repositories;
using ImperaPlus.Domain.Repositories;
using System.Collections.Generic;
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

        public UnitOfWork(ImperaContext context)
        {
            this.context = context;
        }

        public ImperaContext Context
        {
            get
            {
                return this.context;
            }
        }

        public IEnumerable<T> GetChangedEntities<T>() where T : class
        {
            return this.context.ChangeTracker.Entries<T>().Where(x => x.State == EntityState.Modified).Select(x => x.Entity);
        }

        public IGameRepository Games
        {
            get
            {
                return this.games ?? (this.games = new GameRepository(this.context));
            }
        }

        public IChannelRepository Channels
        {
            get
            {
                return this.channels ?? (this.channels = new ChannelRepository(this.context));
            }
        }

        public IChatMessageRepository ChatMessages
        {
            get
            {
                return this.chatMessages ?? (this.chatMessages = new ChatMessageRepository(this.context));
            }
        }

        public IMapTemplateDescriptorRepository MapTemplateDescriptors
        {
            get
            {
                return this.mapTemplates ?? (this.mapTemplates = new MapTemplateDescriptorRepository(this.context));
            }
        }

        public IUserRepository Users
        {
            get
            {
                return this.userRepository ?? (this.userRepository = new UserRepository(this.context));                
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                return this.roleRepository ?? (this.roleRepository = new RoleRepository(this.context));
            }
        }

        public INewsRepository News
        {
            get
            {
                return this.newsRepository ?? (this.newsRepository = new NewsRepository(this.context));
            }
        }

        public IPlayerRepository Players
        {
            get
            {
                return this.playerRepository ?? (this.playerRepository = new PlayerRepository(this.context));
            }
        }

        public ITeamRepository Teams
        {
            get
            {
                return this.teamRepository ?? (this.teamRepository = new TeamRepository(this.context));
            }
        }

        public ILadderRepository Ladders
        {
            get
            {
                return this.ladderRepository ?? (this.ladderRepository = new LadderRepository(this.context));
            }
        }

        public IMessageRepository Messages
        {
            get
            {
                return this.messageRepository ?? (this.messageRepository = new MessageRepository(this.context));
            }
        }

        public ITournamentRepository Tournaments
        {
            get
            {
                return this.tournamentRepository ?? (this.tournamentRepository = new TournamentRepository(this.context));
            }
        }

        public void Commit()
        {
            try
            {
                this.context.SaveChanges();
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
            this.context.Dispose();
        }


        public IGenericRepository<T> GetGenericRepository<T>() where T : class
        {
            return new GenericRepository<T>(this.context);
        }
    }
}
