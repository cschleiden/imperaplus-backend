using Microsoft.EntityFrameworkCore;
using System.Linq;
using ImperaPlus.Domain.Ladders;
using ImperaPlus.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace ImperaPlus.DataAccess.Repositories
{
    public class LadderRepository : GenericRepository<Ladder>, ILadderRepository
    {
        public LadderRepository(DbContext context) 
            : base(context)
        {
        }
        
        public Ladder GetById(Guid id)
        {
            return this.DbSet
                .Include(x => x.Options)
                .Include(x => x.Queue)
                .Include(x => x.Queue.Select(qe => qe.User))
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<LadderStanding> GetStandings(Guid ladderId)
        {
            return this.Context.Set<LadderStanding>()
                .Include(x => x.User)
                .Where(x => x.LadderId == ladderId)
                .OrderByDescending(x => x.Rating);
        }

        public IEnumerable<Ladder> GetAll()
        {
            return this.DbSet
                .Include(x => x.Options)
                .Include(x => x.Queue)
                .Include(x => x.Queue.Select(qe => qe.User));
        }

        public IEnumerable<Ladder> GetActive()
        {
            return this.GetAll()
                .Where(x => x.IsActive);
        }

        public int GetStandingPosition(LadderStanding standing)
        {
            return this.Context.Set<LadderStanding>()
                .OrderByDescending(x => x.Rating)
                .Count(x => x.LadderId == standing.LadderId && x.Rating >= standing.Rating);
        }

        public LadderStanding GetUserStanding(Guid ladderId, string userId)
        {
            return this.Context.Set<LadderStanding>()
                .OrderByDescending(x => x.Rating)
                .FirstOrDefault(x => x.LadderId == ladderId && x.UserId == userId);
        }
    }
}
