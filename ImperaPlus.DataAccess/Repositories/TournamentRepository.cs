﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public Tournament GetById(Guid id, bool includeGames, bool readOnly)
        {
            IQueryable<Tournament> source;

            if (includeGames)
            {
                source = SetWithGames;
            }
            else
            {
                source = Set;
            }

            if (readOnly)
            {
                // source = source.AsNoTracking();
            }

            return source.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Tournament> Get(params TournamentState[] states)
        {
            if (states == null || states.Length == 0)
            {
                states = new[]
                {
                    TournamentState.Open, TournamentState.Groups, TournamentState.Knockout, TournamentState.Closed
                };
            }

            var result = SummarySet
                .Where(x => states.Contains(x.State) &&
                            (!x.EndOfTournament.HasValue || x.EndOfTournament >= DateTime.UtcNow.AddDays(-90)));

            return result;
        }

        public bool ExistsWithName(string name)
        {
            return DbSet.Any(x => x.Name == name);
        }

        public IEnumerable<Tournament> GetAllFull()
        {
            return Set;
        }

        public IEnumerable<Tournament> GetRecentFull()
        {
            return Set.Where(x => x.LastModifiedAt >= DateTime.UtcNow.AddDays(-30));
        }

        public IEnumerable<Game> GetGamesForPairing(Guid pairingId)
        {
            var pairing = Context.Set<TournamentPairing>()
                .Include(x => x.Games)
                .ThenInclude(g => g.Teams)
                .ThenInclude(t => t.Players)
                .ThenInclude(p => p.User)
                .Include(x => x.Games)
                .ThenInclude(g => g.Options)
                .FirstOrDefault(p => p.Id == pairingId);

            return pairing.Games;
        }

        private IQueryable<Tournament> SummarySet =>
            // Include Games/Teams/Players so we can synchronize
            DbSet
                .Include(x => x.Teams)
                .ThenInclude(t => t.Participants)
                .ThenInclude(p => p.User)
                .Include(x => x.Options);

        private IQueryable<Tournament> Set =>
            DbSet
                .Include(x => x.Teams)
                .ThenInclude(t => t.Participants)
                .ThenInclude(p => p.User)
                .Include(x => x.Pairings)
                .Include(x => x.Groups)
                .Include(x => x.Options);

        private IQueryable<Tournament> SetWithGames =>
            // Include Games/Teams/Players so we can synchronize
            DbSet
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
