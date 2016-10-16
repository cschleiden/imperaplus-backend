using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.DataAccess.Repositories
{
    internal class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(DbContext context) 
            : base(context)
        {
        }
    }
}
