using Microsoft.EntityFrameworkCore;
using ImperaPlus.Domain.Chat;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.DataAccess.Repositories
{
    public class ChatMessageRepository : GenericRepository<ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository(DbContext context) : base(context)
        {
        }
    }
}