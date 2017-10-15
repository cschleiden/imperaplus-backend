using System.Collections.Generic;
using ImperaPlus.Domain.Ladders;
using System;

namespace ImperaPlus.Domain.Repositories
{
    public interface ILadderRepository : IGenericRepository<Ladder>
    {
        Ladder GetById(Guid ladderId);

        IEnumerable<LadderStanding> GetStandings(Guid ladderId);

        IEnumerable<Ladder> GetAll();

        IEnumerable<Ladder> GetActive();

        LadderStanding GetUserStanding(Guid ladderId, string userId);

        int GetStandingPosition(LadderStanding standing);

        IEnumerable<Ladder> GetInQueue(string userId);
    }
}
