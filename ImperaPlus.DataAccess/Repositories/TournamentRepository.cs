using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Tournaments;
using ImperaPlus.Domain.Games;

namespace ImperaPlus.DataAccess.Repositories
{
    public class TournamentRepository : GenericRepository<Tournament>, ITournamentRepository
    {
        public TournamentRepository(DbContext context)
            : base(context)
        {
        }

        public Tournament GetById(Guid id, bool includeGames)
        {
            if (includeGames)
            {
                return this.SetWithGames.First(x => x.Id == id);
            }
            else
            {
                return this.Set.First(x => x.Id == id);
            }
        }

        public IQueryable<Tournament> Get(params TournamentState[] states)
        {
            if (states == null || states.Length == 0)
            {
                states = new[] { TournamentState.Open, TournamentState.Groups, TournamentState.Knockout, TournamentState.Closed };
            }

            return this.SummarySet.Where(x => states.Contains(x.State));
        }

        public bool ExistsWithName(string name)
        {
            return this.DbSet.Any(x => x.Name == name);
        }

        public IEnumerable<Tournament> GetAllFull()
        {
            return this.Set;
        }

        public IEnumerable<Game> GetGamesForPairing(Guid pairingId)
        {
            var pairing = this.Context.Set<TournamentPairing>()
                    .Include(x => x.Games)
                        .ThenInclude(g => g.Teams)
                        .ThenInclude(t => t.Players)
                        .ThenInclude(p => p.User)
                    .Include(x => x.Games)
                        .ThenInclude(g => g.Options)
                    .FirstOrDefault(p => p.Id == pairingId);

            return pairing.Games;
        }

        private IQueryable<Tournament> SummarySet
        {
            get
            {
                // Include Games/Teams/Players so we can synchronize
                return this.DbSet
                    .Include(x => x.Teams)
                        .ThenInclude(t => t.Participants)
                            .ThenInclude(p => p.User)
                    .Include(x => x.Options);
            }
        }

        private IQueryable<Tournament> Set
        {
            get
            {
                return this.DbSet
                    .Include(x => x.Teams)
                        .ThenInclude(t => t.Participants)
                            .ThenInclude(p => p.User)
                    .Include(x => x.Pairings)
                    .Include(x => x.Groups)
                    .Include(x => x.Options);
            }
        }

        private IQueryable<Tournament> SetWithGames
        {
            get
            {
                // Include Games/Teams/Players so we can synchronize
                return this.DbSet
                    .Include(x => x.Teams)
                        .ThenInclude(t => t.Participants)
                            .ThenInclude(p => p.User)
                    .Include(x => x.Pairings)
                        .ThenInclude(p => p.Games)
                            .ThenInclude(g => g.Teams)
                                .ThenInclude(t => t.Players)
                    .Include(x => x.Groups)
                    .Include(x => x.Options);
            }
        }
    }
}
