using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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
