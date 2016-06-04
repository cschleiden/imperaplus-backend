using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Repositories;
using System.Data.Entity;

namespace ImperaPlus.DataAccess.Repositories
{
    internal class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        public TeamRepository(DbContext context) 
            : base(context)
        {
        }
    }
}
