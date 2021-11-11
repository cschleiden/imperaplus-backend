using System.Linq;
using ImperaPlus.Domain.Games;
using System.Collections.Generic;
using ImperaPlus.Domain.Enums;

namespace ImperaPlus.Domain.Repositories
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        Game Find(long gameId);

        // Game will be returned not tracked
        Game FindWithHistory(long gameId, long turnNo);

        Game FindByName(string name);

        IQueryable<Game> FindForUser(string userId);

        IQueryable<Game> FindForUserAtTurnReadOnly(string userId);

        /// <summary>
        /// Get number of games where it's the user's turn
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns>Number of games</returns>
        int CountForUserAtTurn(string userId);

        IEnumerable<Game> FindUnscoredLadderGames();

        IQueryable<Game> FindNotHiddenNotOutcomeForUser(string userId, PlayerOutcome outcome);

        IQueryable<Game> FindOpen(string userId);

        IEnumerable<long> FindTimeoutGames();

        int DeleteOpenPasswordFunGames();

        int DeleteEndedGames();

        IEnumerable<Games.Chat.GameChatMessage> GetGameMessages(long gameId, bool isPublic, string userId);
    }
}
