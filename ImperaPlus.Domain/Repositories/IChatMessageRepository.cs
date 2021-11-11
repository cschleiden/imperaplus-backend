using System.Collections.Generic;
using ImperaPlus.Domain.Chat;

namespace ImperaPlus.Domain.Repositories
{
    public interface IChatMessageRepository : IGenericRepository<ChatMessage>
    {
        IEnumerable<ChatMessage> FindForUser(User user);
    }
}
