using System.Collections.Generic;
using ImperaPlus.Domain.Ladders;
using System.Linq;
using System;

namespace ImperaPlus.Domain.Repositories
{
    public interface ILadderRepository : IGenericRepository<Ladder>
    {
        Ladder GetById(Guid ladderId);

        IEnumerable<LadderStanding> GetStandings(Guid ladderId);

        IEnumerable<Ladder> GetAll();

        IEnumerable<Ladder> GetActive();

        LadderStanding GetUserStanding(Guid ladderId, string id);

        int GetStandingPosition(LadderStanding standing);
    }
}
