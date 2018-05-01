using ImperaPlus.Domain.Ladders;
using ImperaPlus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Tournaments;

namespace ImperaPlus.Domain.Tests
{
    public class MockRepository<T> : IGenericRepository<T> where T : class
    {
        private HashSet<T> data = new HashSet<T>();
        
        public Func<T, object[], bool> FindSelector { get; set; }

        public void Add(T item)
        {
            this.data.Add(item);
        }

        public T FindById(params object[] keyValues)
        {
            return this.data.FirstOrDefault(x => this.FindSelector(x, keyValues));
        }

        public IQueryable<T> Query()
        {
            return data.AsQueryable();
        }

        public void Remove(T item)
        {
            this.data.Remove(item);
        }
    }

    public class MockLadderRepository : MockRepository<Ladder>, ILadderRepository
    {
        public MockLadderRepository()
        {
            this.FindSelector = (ladder, keys) =>
            {
                if (keys == null || keys.Length < 1 || !(keys[0] is Guid))
                {
                    return false;
                }

                return ladder.Id == (Guid)keys[0];
            };
        }

        public IQueryable<Ladder> GetActive()
        {
            return this.Query().Where(x => x.IsActive);
        }

        public IEnumerable<Ladder> GetAll()
        {
            return this.Query();
        }

        public Ladder GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Ladder> GetInQueue(string userId)
        {
            throw new NotImplementedException();
        }

        public int GetStandingPosition(LadderStanding standing)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LadderStanding> GetStandings(Guid ladderId)
        {
            throw new NotImplementedException();
        }

        public LadderStanding GetUserStanding(Guid ladderId, string id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Ladder> ILadderRepository.GetActive()
        {
            return this.Query().Where(x => x.IsActive);
        }
    }

    public class MockGamesRepository : MockRepository<Game>, IGameRepository
    {
        public int CountForUserAtTurn(string userId)
        {
            throw new NotImplementedException();
        }

        public Game Find(long gameId)
        {
            throw new NotImplementedException();
        }

        public Game FindByName(string name)
        {
            return this.Query().FirstOrDefault(x => x.Name == name);
        }

        public IQueryable<Game> FindForUser(string userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Game> FindForUserAtTurn(string userId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Game> FindNotHiddenNotOutcomeForUser(string userId, PlayerOutcome outcome)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Game> FindOpen(string userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Game> FindOpenPasswordFunGames()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Game> FindTimeoutGames()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Game> FindUnscoredLadderGames()
        {
            throw new NotImplementedException();
        }

        public Game FindWithHistory(long gameId)
        {
            throw new NotImplementedException();
        }

        public Game FindWithMessages(long gameId)
        {
            throw new NotImplementedException();
        }
    }

    public class MockTournamentRepository : MockRepository<Tournament>, ITournamentRepository
    {
        public bool ExistsWithName(string name)
        {
            return false;
        }

        public IEnumerable<Tournament> Get(params TournamentState[] states)
        {
            return this.Query().Where(x => states.Contains(x.State));
        }

        public IEnumerable<Tournament> GetAllFull()
        {
            return this.Query();
        }

        public Tournament GetById(Guid id)
        {
            return this.FindById(id);
        }

        public IEnumerable<Game> GetGamesForPairing(Guid pairingId)
        {
            return Enumerable.Empty<Game>();
        }
    }
}
