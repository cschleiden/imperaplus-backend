﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Repositories;

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
            return this.FullGameSet.FirstOrDefault(x => x.Id == id);
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
            return
                this.GameSet.Where(
                g => g.Teams.SelectMany(t => t.Players)
                    .Any(p => !p.IsHidden && p.UserId == userId));
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
            return this.DbSet
                .Include(x => x.Teams.Select(t => t.Players.Select(p => p.User)))
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

        public IEnumerable<Game> FindTimeoutGames()
        {            
            return this.FullGameSet.Where(x =>
                x.State == GameState.Active
                && x.LastModifiedAt <= DbFunctions.AddSeconds(DateTime.UtcNow, -x.Options.TimeoutInSeconds));
        }
        
        protected IQueryable<Game> FullGameSet
        {
            get
            {
                return this.GameSet
                    .Include(x => x.Teams.Select(t => t.Players.Select(p => p.User)))
                    .Include(x => x.HistoryEntries)
                    .Include(x => x.Options);
            }
        }

        protected IQueryable<Game> GameSet
        {
            get
            {
                return base.DbSet
                    .Include(x => x.CreatedBy)
                    .Include(x => x.Teams.Select(t => t.Players.Select(p => p.User)))
                    .Include(x => x.Options);
            }
        }        
    }
}