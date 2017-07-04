using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ImperaPlus.Domain.Messages;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.DataAccess.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(DbContext context) 
            : base(context)
        {
        }

        public IEnumerable<Message> GetUnread(string userId)
        {
            return this.DbSet.Where(m => m.Folder == MessageFolder.Inbox && m.OwnerId == userId && !m.IsRead);
        }

        public int CountUnread(string userId)
        {
            return this.DbSet.Count(m => m.Folder == MessageFolder.Inbox && m.OwnerId == userId && !m.IsRead);
        }

        public Message FindById(Guid messageId)
        {
            return this.DbSet.FirstOrDefault(x => x.Id == messageId);
        }

        public IEnumerable<Message> OwnedByUser(string userId)
        {
            return this.DbSet.Where(x => x.OwnerId == userId);
        }
    }
}
