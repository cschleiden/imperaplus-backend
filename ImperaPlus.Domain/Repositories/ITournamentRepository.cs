using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Tournaments;

namespace ImperaPlus.Domain.Repositories
{
    public interface ITournamentRepository : IGenericRepository<Tournament>
    {
        Tournament GetById(Guid id, bool includeGames = false, bool readOnly = false);

        IQueryable<Tournament> Get(params TournamentState[] states);

        bool ExistsWithName(string name);

        IEnumerable<Tournament> GetAllFull();

        IEnumerable<Tournament> GetRecentFull();

        IEnumerable<Game> GetGamesForPairing(Guid pairingId);
    }
}
