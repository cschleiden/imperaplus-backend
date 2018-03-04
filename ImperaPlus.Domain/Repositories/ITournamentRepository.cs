using System;
using System.Collections.Generic;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Tournaments;

namespace ImperaPlus.Domain.Repositories
{
    public interface ITournamentRepository : IGenericRepository<Tournament>
    {
        Tournament GetById(Guid id);

        IEnumerable<Tournament> Get(params TournamentState[] states);

        bool ExistsWithName(string name);

        IEnumerable<Tournament> GetAllFull();

        IEnumerable<Game> GetGamesForPairing(Guid pairingId);
    }
}
