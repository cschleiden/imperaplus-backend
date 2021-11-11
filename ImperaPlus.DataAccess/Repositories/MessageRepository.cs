﻿using System;
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
            return DbSet.Where(m => m.Folder == MessageFolder.Inbox && m.OwnerId == userId && !m.IsRead);
        }

        public int CountUnread(string userId)
        {
            return DbSet.Count(m => m.Folder == MessageFolder.Inbox && m.OwnerId == userId && !m.IsRead);
        }

        public Message FindById(Guid messageId)
        {
            return DbSet.FirstOrDefault(x => x.Id == messageId);
        }

        public IEnumerable<Message> OwnedByUser(string userId)
        {
            return DbSet.Where(x => x.OwnerId == userId);
        }

        public IEnumerable<Message> SentByUser(string userId)
        {
            return DbSet.Where(x => x.FromId == userId);
        }

        public IEnumerable<Message> ReceivedByUser(string userId)
        {
            return DbSet.Where(x => x.RecipientId == userId);
        }
    }
}
