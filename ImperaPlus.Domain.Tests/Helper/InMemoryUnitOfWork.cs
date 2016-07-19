using ImperaPlus.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace ImperaPlus.Domain.Tests.Helper
{
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        public IGameRepository Games
        {
            get { throw new NotImplementedException(); }
        }

        public ITeamRepository Teams
        {
            get { throw new NotImplementedException(); }
        }

        public IPlayerRepository Players
        {
            get { throw new NotImplementedException(); }
        }

        public IMapTemplateDescriptorRepository MapTemplateDescriptors
        {
            get { throw new NotImplementedException(); }
        }

        public IChannelRepository Channels
        {
            get { throw new NotImplementedException(); }
        }

        public IChatMessageRepository ChatMessages
        {
            get { throw new NotImplementedException(); }
        }

        public IUserRepository Users
        {
            get { throw new NotImplementedException(); }
        }

        public IRoleRepository Roles
        {
            get { throw new NotImplementedException(); }
        }

        public INewsRepository News
        {
            get { throw new NotImplementedException(); }
        }

        public ILadderRepository Ladders
        {
            get { throw new NotImplementedException(); }
        }

        public IMessageRepository Messages
        {
            get { throw new NotImplementedException(); }
        }

        public ITournamentRepository Tournaments
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetChangedEntities<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {            
        }

        public IGenericRepository<T> GetGenericRepository<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}
