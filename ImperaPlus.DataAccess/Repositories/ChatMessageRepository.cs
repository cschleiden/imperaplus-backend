using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Chat;
using ImperaPlus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.DataAccess.Repositories
{
    public class ChatMessageRepository : GenericRepository<ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<ChatMessage> FindForUser(User user)
        {
            return DbSet.Where(x => x.CreatedById == user.Id);
        }
    }
}
