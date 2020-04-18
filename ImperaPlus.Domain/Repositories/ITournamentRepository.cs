using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Tournaments;

namespace ImperaPlus.Domain.Repositories
{
    public interface ITournamentRepository : IGenericRepository<Tournament>
    {
        Tournament GetById(Guid id, bool includeGames = false);

        Tournament GetByIdReadOnly(Guid tournamentId);

        IQueryable<Tournament> Get(params TournamentState[] states);

        IQueryable<Tournament> GetReadOnly(params TournamentState[] states);

        bool ExistsWithName(string name);

        IEnumerable<Tournament> GetAllFull();

        IEnumerable<Game> GetGamesForPairing(Guid pairingId);
    }
}
