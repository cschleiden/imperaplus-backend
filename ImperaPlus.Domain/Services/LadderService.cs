using System;
using System.Diagnostics;
using System.Linq;
using ImperaPlus.Domain.Ladders;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Utilities;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Ladders.Events;
using System.Collections.Generic;
using NLog.Fluent;

namespace ImperaPlus.Domain.Services
{
    public interface ILadderService
    {
        void CheckAndCreateMatches();

        void Queue(Guid ladderId, User user);

        void LeaveQueue(Guid ladderId, User user);

        Ladder Create(string name, int numberOfTeams, int numberOfPlayers);

        IEnumerable<Ladder> GetQueuedLadders(User user);
    }

    public class LadderService : ILadderService
    {
        private IGameService gameService;
        private IUnitOfWork unitOfWork;
        private IMapTemplateProvider mapTemplateProvider;
        private IEventAggregator eventAggregator;

        private Object createGamesLock = new Object();

        public LadderService(IUnitOfWork unitOfWork, IGameService gameService, IMapTemplateProvider mapTemplateProvider, IEventAggregator eventAggregator)
        {
            this.unitOfWork = unitOfWork;
            this.gameService = gameService;
            this.mapTemplateProvider = mapTemplateProvider;
            this.eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Checks if enough players for a match have queued up, then create games
        /// </summary>
        public void CheckAndCreateMatches()
        {
            Log.Info().Message("Entering CheckAndCreateMatches").Write();

            lock (this.createGamesLock)
            {
                var ladders = this.unitOfWork.Ladders.GetActive().ToList();

                foreach (var ladder in ladders)
                {
                    Log.Debug().Message("Checking ladder {0} {1}", ladder.Id, ladder.Name).Write();

                    var numberOfRequiredPlayers = ladder.Options.NumberOfTeams * ladder.Options.NumberOfPlayersPerTeam;

                    while (ladder.Queue.Count() >= numberOfRequiredPlayers)
                    {
                        Log.Debug().Message("Found enough players").Write();

                        var game = this.CreateGame(ladder);
                        this.unitOfWork.Games.Add(game);
                        ladder.Games.Add(game);

                        // Add required players from queue to game
                        var queueEntries = ladder.Queue.Take(numberOfRequiredPlayers).ToArray();
                        foreach (var queueEntry in queueEntries.Shuffle())
                        {
                            Debug.Assert(queueEntry.User != null, "User not available for queue entry");

                            // Add user to game
                            game.AddPlayer(queueEntry.User);

                            // Remove user from queue
                            ladder.Queue.Remove(queueEntry);
                            this.unitOfWork.GetGenericRepository<LadderQueueEntry>().Remove(queueEntry);
                        }

                        game.Start(this.mapTemplateProvider.GetTemplate(game.MapTemplateName));

                        this.eventAggregator.Raise(new LadderGameStartedEvent(ladder, game));
                        this.unitOfWork.Commit();

                        Log.Debug().Message("Created game {0}", game.Id).Write();
                    }
                }
            }
        }

        /// <summary>
        /// Create new ladeder
        /// </summary>
        public Ladder Create(string name, int numberOfTeams, int numberOfPlayers)
        {            
            var ladder = new Ladder(name, numberOfTeams, numberOfPlayers);

            this.unitOfWork.Ladders.Add(ladder);

            return ladder;
        }

        /// <summary>
        /// Queue player for ladder
        /// </summary>        
        public void Queue(Guid ladderId, User user)
        {
            Ladder ladder = this.unitOfWork.Ladders.Query().First(l => l.Id == ladderId);

            ladder.QueueUser(user);
        }

        /// <summary>
        /// Leave the queue, if currently in it
        /// </summary>
        public void LeaveQueue(Guid ladderId, User user)
        {
            Ladder ladder = this.unitOfWork.Ladders.GetById(ladderId);

            ladder.QueueLeaveUser(user);
        }

        /// <summary>
        /// Get all ladders where the user is currently queued
        /// </summary>
        /// <param name="user">User fo check for</param>
        public IEnumerable<Ladder> GetQueuedLadders(User user)
        {
            return this.unitOfWork.Ladders.GetInQueue(user.Id);
        }

        /// <summary>
        /// Create a game for the given ladder
        /// </summary>        
        protected virtual Games.Game CreateGame(Ladder ladder)
        {
            var systemUser = this.unitOfWork.Users.FindByName("System");

            var mapTemplate = ladder.GetMapTemplateForGame();

            var game = gameService.Create(
                Enums.GameType.Ranking,
                systemUser,
                ladder.GetGameName(),
                mapTemplate,
                ladder.Options);            

            game.Ladder = ladder;
            game.LadderId = ladder.Id;

            return game;
        }
    }
}
