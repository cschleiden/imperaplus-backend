﻿using System;
using System.Collections.Generic;
using ImperaPlus.Domain.Messages;

namespace ImperaPlus.Domain.Repositories
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Message FindById(Guid messageId);

        /// <summary>
        /// Get unread messages for given user in Inbox
        /// </summary>
        IEnumerable<Message> GetUnread(string userId);

        /// <summary>
        /// Get unread count for given user in Inbox
        /// </summary>
        /// <param name="userId">User to check for</param>
        /// <returns>Number of unread messages</returns>
        int CountUnread(string userId);

        IEnumerable<Message> SentByUser(string userId);

        IEnumerable<Message> ReceivedByUser(string userId);

        IEnumerable<Message> OwnedByUser(string userId);
    }
}
