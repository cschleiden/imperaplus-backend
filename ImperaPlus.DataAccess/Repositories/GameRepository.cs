using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Repositories;
using Z.EntityFramework.Plus;
using ImperaPlus.Domain.Games.History;
using ImperaPlus.Domain.Games.Chat;

namespace ImperaPlus.DataAccess.Repositories
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        public GameRepository(ImperaContext context)
            : base(context)
        {
        }

        public Game Find(long id)
        {
            return GameSet.FirstOrDefault(x => x.Id == id);
        }

        public Game FindWithHistory(long id, long turnNo)
        {
            var game = GameSet.FirstOrDefault(x => x.Id == id);
            base.Context.Set<HistoryEntry>().Where(x => x.GameId == id && x.TurnNo >= turnNo).Load();
            return game;
        }

        public Game FindByName(string name)
        {
            return GameSet.FirstOrDefault(x => x.Name == name);
        }

        public IQueryable<Game> FindForUser(string userId)
        {
            return GameSet.Where(
                g => g.Teams.Any(t => t.Players.Any(p => !p.IsHidden && p.UserId == userId)));
        }

        public IQueryable<Game> FindForUserAtTurnReadOnly(string userId)
        {
            return GameSet
                .Where(g => g.Teams
                    .SelectMany(t => t.Players)
                    .First(p => p.Id == g.CurrentPlayerId)
                    .UserId == userId && g.State == GameState.Active);
        }

        public int CountForUserAtTurn(string userId)
        {
            return DbSet.Count(g =>
                g.Teams.SelectMany(t => t.Players).FirstOrDefault(p => p.Id == g.CurrentPlayerId).UserId == userId &&
                g.State == GameState.Active);
        }

        public IQueryable<Game> FindNotHiddenNotOutcomeForUser(string userId, PlayerOutcome outcome)
        {
            return GameSet
                .Where(g =>
                    g.Teams.Any(t =>
                        t.Players.Any(p =>
                            p.UserId == userId
                            && !p.IsHidden
                            && p.Outcome != outcome)));
        }

        public IQueryable<Game> FindOpen(string userId)
        {
            return GameSet
                .Where(g => g.State == GameState.Open
                            && g.Teams.SelectMany(t => t.Players).All(p => p.UserId != userId));
        }

        public IEnumerable<long> FindTimeoutGames()
        {
            return DbSet.Where(x =>
                    x.State == GameState.Active
                    && x.LastTurnStartedAt <= DateTime.UtcNow.AddSeconds(-x.Options.TimeoutInSeconds))
                .Select(x => x.Id);
        }

        public IEnumerable<Game> FindUnscoredLadderGames()
        {
            return GameSet
                .Where(x =>
                    x.State == GameState.Ended
                    && x.LadderId != null
                    && (x.LadderScored == null || x.LadderScored == false))
                .OrderByDescending(x => x.LastTurnStartedAt);
        }

        public int DeleteOpenPasswordFunGames()
        {
            return DbSet
                .Where(x => x.State == GameState.Open && x.Type == GameType.Fun && x.Password != null &&
                            x.CreatedAt <= DateTime.UtcNow.AddDays(-5))
                .Delete();
        }

        public int DeleteEndedGames()
        {
            return DbSet
                .Where(x =>
                    // Ignore tournament games for now
                    x.Type != GameType.Tournament
                    && x.State == GameState.Ended
                    && x.LastModifiedAt <= DateTime.UtcNow.AddDays(-10))
                .Take(500)
                .Delete(x => x.BatchSize = 100);
        }

        public IEnumerable<GameChatMessage> GetGameMessages(long gameId, bool isPublic, string userId)
        {
            var messages = Context.Set<GameChatMessage>()
                .Where(x => x.GameId == gameId);

            if (isPublic)
            {
                messages = messages.Where(x => x.TeamId == null);
            }
            else
            {
                messages = messages.Where(x => x.Team.Players.Any(p => p.UserId == userId));
            }

            return messages;
        }

        protected IQueryable<Game> GameSet =>
            base.DbSet
                .Include(g => g.Teams)
                .ThenInclude(t => t.Players)
                .ThenInclude(p => p.User)
                .Include(x => x.CreatedBy)
                .Include(x => x.Options);
    }
}
