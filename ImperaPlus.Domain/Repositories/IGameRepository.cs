using System.Linq;
using ImperaPlus.Domain.Games;
using System.Collections.Generic;
using ImperaPlus.Domain.Enums;

namespace ImperaPlus.Domain.Repositories
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        Game Find(long gameId);

        Game FindWithMessages(long gameId);

        Game FindWithHistory(long gameId);

        Game FindByName(string name);

        IQueryable<Game> FindForUser(string userId);

        IEnumerable<Game> FindForUserAtTurn(string userId);

        /// <summary>
        /// Get number of games where it's the user's turn
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns>Number of games</returns>
        int CountForUserAtTurn(string userId);

        IEnumerable<Game> FindUnscoredLadderGames();

        IQueryable<Game> FindNotHiddenNotOutcomeForUser(string userId, PlayerOutcome outcome);

        IQueryable<Game> FindOpen(string userId);

        IEnumerable<Game> FindTimeoutGames();
    }
}