using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Ladders;
using ImperaPlus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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
            return this.Set
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Ladder> GetInQueue(string userId)
        {
            return this.DbSet
                .Include(x => x.Queue)
                .Where(x => x.Queue.Any(qe => qe.UserId == userId));
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
            return this.Set;
        }

        public IEnumerable<Ladder> GetActive()
        {
            return this.Set
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

        private IQueryable<Ladder> Set
        {
            get
            {
                return this.DbSet
                    .Include(x => x.Options)
                    .Include(x => x.Queue)
                        .ThenInclude(qe => qe.User);
            }
        }
    }
}
