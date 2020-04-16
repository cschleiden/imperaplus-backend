using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Repositories;
using Z.EntityFramework.Plus;

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
            return this.GameSet.FirstOrDefault(x => x.Id == id);
        }

        public Game FindWithHistory(long id)
        {
            return this.FullGameSet.FirstOrDefault(x => x.Id == id);
        }

        public Game FindWithMessages(long id)
        {
            return this.GameSet.Include(x => x.ChatMessages).FirstOrDefault(x => x.Id == id);
        }

        public Game FindByName(string name)
        {
            return this.DbSet.FirstOrDefault(x => x.Name == name);
        }

        public IQueryable<Game> FindForUser(string userId)
        {
            return this.GameSet.Where(
                    g => g.Teams.Any(t => t.Players.Any(p => !p.IsHidden && p.UserId == userId)));
        }

        public IEnumerable<Game> FindForUserAtTurn(string userId)
        {
            return
                this.GameSet.Where(g => g.Teams.SelectMany(t => t.Players).FirstOrDefault(p => p.Id == g.CurrentPlayerId).UserId == userId && g.State == GameState.Active);
        }

        public int CountForUserAtTurn(string userId)
        {
            return this.DbSet.Count(g => g.Teams.SelectMany(t => t.Players).FirstOrDefault(p => p.Id == g.CurrentPlayerId).UserId == userId && g.State == GameState.Active);
        }

        public IQueryable<Game> FindNotHiddenNotOutcomeForUser(string userId, PlayerOutcome outcome)
        {
            return this.GameSet
                .Where(g =>
                    g.Teams.Any(t =>
                        t.Players.Any(p =>
                            p.UserId == userId
                            && !p.IsHidden
                            && p.Outcome != outcome)));
        }

        public IQueryable<Game> FindOpen(string userId)
        {
            return this.GameSet
                .Where(g => g.State == GameState.Open
                    && g.Teams.SelectMany(t => t.Players).All(p => p.UserId != userId));
        }

        public IEnumerable<long> FindTimeoutGames()
        {
            return this.DbSet.Where(x =>
                x.State == GameState.Active
                && x.LastTurnStartedAt <= DateTime.UtcNow.AddSeconds(-x.Options.TimeoutInSeconds))
                .Select(x => x.Id);
        }

        public IEnumerable<Game> FindUnscoredLadderGames()
        {
            return this.GameSet
                .Where(x =>
                    x.State == GameState.Ended
                    && x.LadderId != null
                    && (x.LadderScored == null || x.LadderScored == false))
                .OrderByDescending(x => x.LastTurnStartedAt);
        }

        public int DeleteOpenPasswordFunGames()
        {
            return this.DbSet
                .Where(x => x.State == GameState.Open && x.Password != null && x.CreatedAt <= DateTime.UtcNow.AddDays(-5))
                .Delete();
        }

        public int DeleteEndedGames()
        {
            return this.DbSet
                .Where(x => x.State == GameState.Ended && x.Type == GameType.Fun && x.LastModifiedAt <= DateTime.UtcNow.AddDays(-10))
                .Delete(x => x.BatchSize = 20);
        }

        protected IQueryable<Game> FullGameSet
        {
            get
            {
                return this.GameSet
                    .Include(x => x.HistoryEntries);
            }
        }

        protected IQueryable<Game> GameSet
        {
            get
            {
                return base.DbSet
                    .Include(x => x.CreatedBy)
                    .Include(x => x.Teams)
                        .ThenInclude(t => t.Players)
                            .ThenInclude(p => p.User)
                    .Include(x => x.Options);
            }
        }
    }
}