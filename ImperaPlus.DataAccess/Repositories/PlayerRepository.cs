using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Repositories;
using System.Data.Entity;

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
