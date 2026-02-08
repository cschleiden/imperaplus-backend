using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ImperaPlus.Domain.Annotations;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Games.Chat;
using ImperaPlus.Domain.Games.Events;
using ImperaPlus.Domain.Games.History;
using ImperaPlus.Domain.Ladders;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Utilities;
using ImperaPlus.Domain.VictoryConditions;
using ImperaPlus.Utils;
using NLog.Fluent;

namespace ImperaPlus.Domain.Games
{
    public class Game : Entity, IIdentifiableEntity, IChangeTrackedEntity, IOwnedEntity
    {
        [UsedImplicitly]
        protected Game()
        {
            ChatMessages = new HashSet<GameChatMessage>();
            HistoryEntries = new List<HistoryEntry>();
            GameHistory = new GameHistory(this);
            Teams = new HashSet<Team>();
        }

        public Game(
            User user,
            GameType type,
            string name,
            string password,
            string mapTemplateName,
            int timeoutInSeconds,
            int numberOfTeams,
            int numberOfPlayersPerTeam,
            IEnumerable<VictoryConditionType> victoryConditions,
            IEnumerable<VisibilityModifierType> visibilityModifier)
            : this(user, type, name, password, mapTemplateName,
                new GameOptions
                {
                    NumberOfPlayersPerTeam = numberOfPlayersPerTeam,
                    NumberOfTeams = numberOfTeams,
                    TimeoutInSeconds = timeoutInSeconds
                })
        {
            Options.VictoryConditions.AddRange(victoryConditions);
            Options.VisibilityModifier.AddRange(visibilityModifier);
        }

        public Game(
            User user,
            GameType type,
            string name,
            string password,
            string mapTemplateName,
            GameOptions options)
            : this()
        {
            Type = type;
            Name = name;
            Password = password;

            if (Password != null && Type != GameType.Fun)
            {
                throw new DomainException(ErrorCode.GamePasswordOnlyAllowedForFun,
                    "Passwords for games are only allowed for fun games");
            }

            CreatedBy = user;
            CreatedAt = DateTime.UtcNow;

            MapTemplateName = mapTemplateName;

            Options = options;

            State = GameState.Open;
            PlayState = PlayState.None;
        }

        public long Id { get; set; }

        /// <summary>
        /// Used for concurrency checks
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime LastTurnStartedAt { get; set; }

        public virtual Ladder Ladder { get; set; }
        public Guid? LadderId { get; set; }

        /// <summary>
        /// Whether the game has been updated for the ladder score
        /// </summary>
        public bool? LadderScored { get; set; }

        public Guid? TournamentPairingId { get; set; }

        public GameType Type { get; private set; }

        public string Name { get; private set; }

        public string Password { get; private set; }

        public virtual Map Map { get; set; }

        public virtual ICollection<Team> Teams { get; private set; }

        [NotMapped] public GameHistory GameHistory { get; private set; }

        public virtual ICollection<GameChatMessage> ChatMessages { get; private set; }

        public string MapTemplateName { get; private set; }

        public GameState State { get; internal set; }

        public PlayState PlayState { get; internal set; }

        public virtual ICollection<HistoryEntry> HistoryEntries { get; private set; }

        public Guid? CurrentPlayerId { get; set; }

        [NotMapped]
        public Player CurrentPlayer
        {
            get
            {
                if (!CurrentPlayerId.HasValue)
                {
                    return null;
                }

                return Teams.SelectMany(x => x.Players).FirstOrDefault(p => p.Id == CurrentPlayerId.Value);
            }
        }

        public long OptionsId { get; private set; }
        public virtual GameOptions Options { get; private set; }

        public int TurnCounter { get; set; }

        public int AttacksInCurrentTurn { get; set; }

        public int MovesInCurrentTurn { get; set; }

        public bool CardDistributed { get; set; }

        /// <summary>
        /// Gets a value indicating whether the game can start
        /// </summary>
        public bool CanStart
        {
            get
            {
                var players = Teams.SelectMany(x => x.Players).Count();

                return players == Options.PlayerCount && State == GameState.Open;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the game can be deleted
        /// </summary>
        public bool CanBeDeleted => State == GameState.Open;

        /// <summary>
        /// Gets a value indicating whether players can leave the game
        /// </summary>
        [NotMapped]
        public bool CanLeave => State == GameState.Open;

        public void ResetMapTracking()
        {
            if (Map != null)
            {
                Map.ResetTracking();
            }
        }

        /// <summary>
        /// Adds a new team
        /// </summary>
        /// <returns>New team</returns>
        public Team AddTeam()
        {
            if (Teams.Count() >= Options.NumberOfTeams)
            {
                throw new DomainException(ErrorCode.TooManyTeams,
                    "Cannot create team, there are too many teams already");
            }

            var team = new Team(this);

            Teams.Add(team);

            return team;
        }

        /// <summary>
        /// Post a new message to the game chat
        /// </summary>
        /// <param name="user">User for which to post message, has to be player in game</param>
        /// <param name="text">Message text</param>
        /// <param name="isPublic">Value indicating whether the message is for all players</param>
        /// <returns>Newly created message</returns>
        public GameChatMessage PostMessage(User user, string text, bool isPublic)
        {
            Require.NotNull(user, "user");
            Require.NotNullOrEmpty(text, "text");

            RequireGameStarted();

            var player = GetPlayerForUser(user.Id);

            Team team = null;
            if (!isPublic)
            {
                team = player.Team;
            }

            var message = new GameChatMessage(this, user, team, text);

            ChatMessages.Add(message);

            return message;
        }

        public IEnumerable<GameChatMessage> GetMessages(User user, bool isPublic)
        {
            Require.NotNull(user, "user");

            var player = GetPlayerForUser(user.Id);

            IEnumerable<GameChatMessage> messages;
            if (isPublic)
            {
                messages = ChatMessages.Where(x => x.TeamId == null);
            }
            else
            {
                messages = ChatMessages.Where(x => x.TeamId == player.TeamId);
            }

            return messages.OrderBy(x => x.TeamId).OrderBy(x => x.DateTime).ToList();
        }

        /// <summary>
        /// Add player and implicitly create team
        /// </summary>
        public Player AddPlayer(User user, string password = null)
        {
            Require.NotNull(user, "user");

            if (IsPasswordProtected && !string.Equals(password, Password, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new DomainException(ErrorCode.GamePasswordNotCorrect,
                    "Game is password protected and given password does not match");
            }

            if (Teams.SelectMany(x => x.Players).Any(x => x.UserId == user.Id))
            {
                throw new DomainException(ErrorCode.PlayerAlreadyJoined, "Player has already joined this game.");
            }

            Team team;
            if (Teams.Count < Options.NumberOfTeams)
            {
                team = AddTeam();
            }
            else
            {
                team = Teams.FirstOrDefault(x => x.Players.Count() < Options.NumberOfPlayersPerTeam);
                if (team == null)
                {
                    throw new DomainException(ErrorCode.TooManyTeams, "All teams have enough players");
                }
            }

            return team.AddPlayer(user);
        }

        /// <summary>
        /// Remove player from team and game
        /// </summary>
        /// <param name="user"></param>
        public void Leave(User user)
        {
            Require.NotNull(user, "user");

            if (!CanLeave)
            {
                throw new DomainException(ErrorCode.CannotLeaveGame, "Game state does not allow leaving");
            }

            if (user.Id == CreatedBy.Id)
            {
                throw new DomainException(ErrorCode.CannotLeaveGame, "Game creator cannot leave game");
            }

            var player = GetPlayerForUser(user.Id);
            var team = player.Team;

            team.RemovePlayer(player);

            if (!team.Players.Any())
            {
                // Remove empty team
                Teams.Remove(team);
            }

            // Recalculate PlayOrder to maintain sequential values
            RecalculatePlayOrder();
        }

        /// <summary>
        /// Recalculates PlayOrder for all teams and players to ensure sequential ordering
        /// </summary>
        private void RecalculatePlayOrder()
        {
            var orderedTeams = Teams.OrderBy(t => t.PlayOrder).ToList();
            var teamIndex = 0;
            
            foreach (var currentTeam in orderedTeams)
            {
                currentTeam.PlayOrder = teamIndex;
                
                var orderedPlayersInTeam = currentTeam.Players.OrderBy(p => p.PlayOrder).ToList();
                var playerIndex = 0;
                
                foreach (var currentPlayer in orderedPlayersInTeam)
                {
                    currentPlayer.PlayOrder = teamIndex + (playerIndex * Options.NumberOfTeams);
                    playerIndex++;
                }
                
                teamIndex++;
            }
        }

        /// <summary>
        /// Start the game
        /// </summary>
        public void Start(MapTemplate mapTemplate, IRandomGen random)
        {
            if (!CanStart)
            {
                throw new DomainException(ErrorCode.CannotStartGame, "Cannot start");
            }

            StartedAt = DateTime.UtcNow;
            State = GameState.Active;

            TraceContext.Trace("Create Map from Template", () =>
            {
                Map = Map.CreateFromTemplate(this, mapTemplate);
            });
            TraceContext.Trace(
                "Distribute countries to teams",
                () => Map.Distribute(Options, Teams, mapTemplate, Options.MapDistribution, random)
            );

            // Determine player order
            DeterminePlayOrder(random);

            // Set current player
            var currentTeam = Teams.RandomElement(random);
            var currentPlayer = currentTeam.Players.First();

            CurrentPlayerId = currentPlayer.Id;

            // Allow victory conditions to modify the game state
            foreach (var victoryCondition in Options.VictoryConditions)
            {
                var victoryConditionImpl = VictoryConditionFactory.Create(victoryCondition);
                victoryConditionImpl.Initialize(this, random);
            }

            // Record in history
            GameHistory.RecordStart();

            TurnCounter = 1;

            ResetTurn();

            EventQueue.Raise(new GameStartedEvent(this));
        }

        /// <summary>
        /// Ends the current turn and switches to the next player
        /// </summary>
        public void EndTurn()
        {
            if (State != GameState.Active)
            {
                return;
            }

            GameHistory.RecordEndTurn();
            TurnCounter++;

            CheckForVictory();
            if (State != GameState.Active)
            {
                // Game might have ended
                return;
            }

            // Go to next player
            Player nextPlayer = null;
            for (var i = 1; i < 2 * Options.PlayerCount && nextPlayer == null && nextPlayer != CurrentPlayer; ++i)
            {
                var nextPlayerOrder = (CurrentPlayer.PlayOrder + i) % Options.PlayerCount;
                nextPlayer = Teams
                    .SelectMany(x => x.Players)
                    .OrderBy(x => x.PlayOrder)
                    .FirstOrDefault(x => x.State == PlayerState.Active && x.PlayOrder >= nextPlayerOrder);
            }

            if (nextPlayer == null)
            {
                Log.Fatal().Message("Cannot find active player for game {0}", Id).Write();
                throw new DomainException(ErrorCode.GenericError, "Cannot find active player");
            }

            CurrentPlayerId = nextPlayer.Id;

            ResetTurn();
            EventQueue.Raise(new TurnEndedEvent(this));
        }

        private void ResetTurn()
        {
            CurrentPlayer.Bonus = 0;

            AttacksInCurrentTurn = 0;
            MovesInCurrentTurn = 0;

            CardDistributed = false;

            PlayState = PlayState.PlaceUnits;

            LastTurnStartedAt = DateTime.UtcNow;
        }

        public int GetUnitsToPlace(MapTemplate mapTemplate, Player player)
        {
            // Base units as configured
            var unitsToPlace = Options.NewUnitsPerTurn;

            if (player.PlacedInitialUnits)
            {
                // Add units from number of occupied countries
                unitsToPlace += player.Countries.Count() / 3;

                // Bonus from continents
                unitsToPlace += mapTemplate.CalculateBonus(player.Countries.Select(x => x.CountryIdentifier));

                // Add any previous bonus
                unitsToPlace += player.Bonus;
            }

            return unitsToPlace;
        }

        public void ExchangeCards()
        {
            RequireGameActive();

            if (PlayState != PlayState.PlaceUnits)
            {
                throw new DomainException(ErrorCode.ExchangingCardsNotAllowed, "Exchanging cards is not allowed");
            }

            var unitsReceived = CurrentPlayer.ExchangeCards();
            if (unitsReceived > 0)
            {
                GameHistory.RecordCardExchange(CurrentPlayer, CurrentPlayer.Bonus);
            }
        }

        public void PlaceUnits(MapTemplate mapTemplate, IEnumerable<Tuple<string, int>> countries)
        {
            RequireGameActive();

            if (PlayState != PlayState.PlaceUnits)
            {
                throw new DomainException(ErrorCode.PlacingNotAllowed, "Placing units is not allowed");
            }

            var unitsToPlace = countries.Sum(x => x.Item2);
            var unitsAvailable = GetUnitsToPlace(mapTemplate, CurrentPlayer);
            if (unitsToPlace > unitsAvailable)
            {
                throw new DomainException(ErrorCode.PlacingMoreUnitsThanAvailable,
                    "Cannot place more units than available");
            }
            else if (unitsToPlace < unitsAvailable)
            {
                throw new DomainException(ErrorCode.PlacingLessUnitsThanAvailable,
                    "Cannot place less units than available");
            }

            foreach (var place in countries)
            {
                var country = Map.GetCountry(place.Item1);

                // Check ownership
                var owner = GetPlayerById(country.PlayerId);
                if (owner.TeamId != CurrentPlayer.TeamId)
                {
                    throw new DomainException(ErrorCode.PlacingToForeignCountry,
                        "Country does not belong to current team");
                }

                country.PlaceUnits(place.Item2);

                GameHistory.RecordPlace(CurrentPlayer, country.CountryIdentifier, place.Item2);
            }

            if (!CurrentPlayer.PlacedInitialUnits)
            {
                CurrentPlayer.PlacedInitialUnits = true;

                // In the first turn, players are only allowed to place
                EndTurn();
            }
            else
            {
                PlayState = PlayState.Attack;
            }
        }

        public void Attack(
            IAttackService attackService,
            IRandomGen randomGen,
            MapTemplate mapTemplate,
            string sourceCountryIdentifier,
            string destCountryIdentifier,
            int numberOfUnits)
        {
            RequireGameActive();

            if (PlayState != PlayState.Attack)
            {
                throw new DomainException(ErrorCode.AttackingNotPossible, "Cannot attack, state incorrect");
            }

            var sourceCountry = Map.GetCountry(sourceCountryIdentifier);
            var destCountry = Map.GetCountry(destCountryIdentifier);

            // Check connection
            if (!mapTemplate.AreConnected(sourceCountryIdentifier, destCountryIdentifier))
            {
                throw new DomainException(ErrorCode.CountriesNotConnected,
                    "There is no connection between those countries");
            }

            // Check ownership
            if (sourceCountry.TeamId != CurrentPlayer.TeamId)
            {
                throw new DomainException(ErrorCode.OriginCountryNotOwnedByTeam,
                    "Can only initiate actions from countries that belong to the same team");
            }

            if (sourceCountry.PlayerId == destCountry.PlayerId)
            {
                throw new DomainException(ErrorCode.AttackOwnCountries, "Cannot attack own countries");
            }

            if (numberOfUnits <= 0 || sourceCountry.Units - numberOfUnits < Options.MinUnitsPerCountry)
            {
                throw new DomainException(ErrorCode.NotEnoughUnits, "Cannot attack with that many units");
            }

            var otherPlayer = GetPlayerById(destCountry.PlayerId);

            var result = attackService.Attack(
                numberOfUnits, destCountry.Units, out var attackerUnitsLost, out var defenderUnitsLost);

            if (result)
            {
                // Attack was successful
                destCountry.Units = numberOfUnits - attackerUnitsLost;

                // Changing ownership removes capitals
                if (destCountry.Flags.HasFlag(CountryFlags.Capital))
                {
                    GameHistory.RecordCapitalLost(CurrentPlayer, destCountry.CountryIdentifier);
                    otherPlayer.ForfeitCountries();
                }

                Map.UpdateOwnership(otherPlayer, CurrentPlayer, destCountry);

                DistributeCard(randomGen);
            }
            else
            {
                // Attack failed
                destCountry.Units -= defenderUnitsLost;
            }

            // Reduce units in this country in any case
            sourceCountry.Units -= numberOfUnits;

            GameHistory.RecordAttack(
                CurrentPlayer, otherPlayer,
                sourceCountryIdentifier, destCountryIdentifier,
                numberOfUnits, attackerUnitsLost, defenderUnitsLost,
                result);

            // Reduce number of attacks left
            AttacksInCurrentTurn++;

            // Check for victory
            CheckForVictory();

            if (AttacksInCurrentTurn >= Options.AttacksPerTurn)
            {
                EndAttack();
            }
        }

        public void EndAttack()
        {
            PlayState = PlayState.Move;
        }

        private void DistributeCard(IRandomGen randomGen)
        {
            if (!CardDistributed)
            {
                CardDistributed = true;

                if (CurrentPlayer.Cards.Count() < Options.MaximumNumberOfCards)
                {
                    var existingCards = new List<BonusCard>(CurrentPlayer.Cards);

                    var cardToDistribute = randomGen.GetNext(0, 2);
                    existingCards.Add((BonusCard)cardToDistribute);

                    CurrentPlayer.Cards = existingCards.ToArray();
                }
            }
        }

        internal void CheckForVictory()
        {
            foreach (var victoryCondition in Options.VictoryConditions)
            {
                var victoryConditionImpl = VictoryConditionFactory.Create(victoryCondition);

                foreach (var player in Teams.SelectMany(t => t.Players).Where(p => p.State == PlayerState.Active))
                {
                    var result = victoryConditionImpl.Evaluate(player, Map);
                    switch (result)
                    {
                        case VictoryConditionResult.Defeat:
                            {
                                player.Outcome = PlayerOutcome.Defeated;
                                player.State = PlayerState.InActive;

                                GameHistory.RecordPlayerDefeated(player);
                                break;
                            }

                        case VictoryConditionResult.TeamDefeat:
                            {
                                foreach (var teamPlayer in player.Team.Players)
                                {
                                    teamPlayer.Outcome = PlayerOutcome.Defeated;
                                    teamPlayer.State = PlayerState.InActive;

                                    GameHistory.RecordPlayerDefeated(teamPlayer);
                                }

                                break;
                            }

                        case VictoryConditionResult.Victory:
                            {
                                player.Outcome = PlayerOutcome.Won;
                                player.State = PlayerState.InActive;

                                break;
                            }

                        case VictoryConditionResult.TeamVictory:
                            {
                                foreach (var teamPlayer in player.Team.Players)
                                {
                                    teamPlayer.Outcome = PlayerOutcome.Won;
                                    teamPlayer.State = PlayerState.InActive;
                                }

                                break;
                            }
                    }
                }
            }

            // Update team status
            var activeTeams = Teams.Where(x => x.Players.Any(p => !p.HasLost));
            if (activeTeams.Count() <= 1)
            {
                // Only one team has survived, all players in this team have won
                foreach (var player in activeTeams.Single().Players)
                {
                    player.Outcome = PlayerOutcome.Won;
                    player.State = PlayerState.InActive;

                    GameHistory.RecordPlayerWon(player);
                    // TODO: CS: Generate event
                }

                End();
            }
        }

        public void End()
        {
            // Set this first to prevent EndTurn from determining new player
            State = GameState.Ended;

            EndTurn();

            GameHistory.RecordEnd();

            EventQueue.Raise(new GameEndedEvent(this));
        }

        public Player GetPlayerForUser(string userId)
        {
            var player = Teams.SelectMany(x => x.Players).FirstOrDefault(x => x.UserId == userId);

            if (null == player)
            {
                throw new DomainException(ErrorCode.PlayerNotInGame, "User is not a player in this game");
            }

            return player;
        }

        public virtual Player GetPlayerById(Guid id)
        {
            if (Guid.Empty == id)
            {
                // Neutral player
                return null;
            }

            var player = Teams.SelectMany(x => x.Players).FirstOrDefault(x => x.Id == id);
            if (null == player)
            {
                throw new DomainException(ErrorCode.PlayerNotInGame, "Cannot find player with id");
            }

            return player;
        }

        public void ProcessTimeouts()
        {
            if (DateTime.UtcNow - LastTurnStartedAt > TimeSpan.FromSeconds(Options.TimeoutInSeconds))
            {
                ++CurrentPlayer.Timeouts;
                GameHistory.RecordTimeout();

                if (Options.MaximumTimeoutsPerPlayer > 0
                    && CurrentPlayer.Timeouts > Options.MaximumTimeoutsPerPlayer)
                {
                    // TODO: CS: Refactor?
                    CurrentPlayer.State = PlayerState.InActive;
                    CurrentPlayer.Outcome = PlayerOutcome.Defeated;

                    foreach (var country in CurrentPlayer.Countries.ToArray())
                    {
                        Map.UpdateOwnership(null, country);
                        GameHistory.RecordOwnershipChange(CurrentPlayer, null, country.CountryIdentifier);
                    }

                    CheckForVictory();
                }

                EndTurn();
            }
        }

        /// <summary>
        /// Gets the number of slots this game occupies for each player
        /// </summary>
        public int RequiredSlots => 1 /* + MapSlots*/;

        public bool IsPasswordProtected => Password != null;

        public void Move(
            MapTemplate mapTemplate,
            string sourceCountryIdentifier,
            string destCountryIdentifier,
            int numberOfUnits)
        {
            RequireGameActive();

            // Check connection
            if (!mapTemplate.AreConnected(sourceCountryIdentifier, destCountryIdentifier))
            {
                throw new DomainException(ErrorCode.CountriesNotConnected, "Countries are not connected");
            }

            var sourceCountry = Map.GetCountry(sourceCountryIdentifier);
            var destCountry = Map.GetCountry(destCountryIdentifier);

            if (numberOfUnits <= 0 || sourceCountry.Units - numberOfUnits < Options.MinUnitsPerCountry)
            {
                throw new DomainException(ErrorCode.NotEnoughUnits, "Cannot move that many units");
            }

            // Check ownership
            if (sourceCountry.TeamId != CurrentPlayer.TeamId)
            {
                throw new DomainException(ErrorCode.OriginCountryNotOwnedByTeam,
                    "Can only initiate actions from countries that belong to the same team");
            }

            if (sourceCountry.TeamId != destCountry.TeamId)
            {
                throw new DomainException(ErrorCode.MoveOwnCountries,
                    "Units can only be moved between countries that belong to the same team");
            }

            if (PlayState != PlayState.Move)
            {
                if (PlayState == PlayState.Attack)
                {
                    // Switch automatically to move and end attacking
                    PlayState = PlayState.Move;
                }
                else
                {
                    throw new DomainException(ErrorCode.MovingNotPossible, "Cannot move, state incorrect");
                }
            }

            // Perform move
            sourceCountry.Units -= numberOfUnits;
            destCountry.Units += numberOfUnits;

            GameHistory.RecordMove(
                CurrentPlayer,
                sourceCountry.CountryIdentifier, destCountry.CountryIdentifier, numberOfUnits);

            MovesInCurrentTurn++;
            if (MovesInCurrentTurn >= Options.MovesPerTurn)
            {
                PlayState = PlayState.Done;
            }
        }

        private void DeterminePlayOrder(IRandomGen random)
        {
            // Desired outcome:
            // t1: p1 - 0
            // t2: p1 - 1
            // t1: p2 - 2
            // t2: p2 - 3

            var shuffledTeams = Teams.Shuffle(random).ToArray();
            var teamIdx = 0;
            foreach (var shuffledTeam in shuffledTeams)
            {
                shuffledTeam.PlayOrder = teamIdx;

                var playerIdx = 0;
                foreach (var player in shuffledTeam.Players)
                {
                    player.PlayOrder = teamIdx + playerIdx * Options.NumberOfTeams;

                    ++playerIdx;
                }

                ++teamIdx;
            }
        }

        private void RequireGameActive()
        {
            if (State != GameState.Active)
            {
                throw new DomainException(ErrorCode.GameNotActive, "Game is not active");
            }
        }

        private void RequireGameStarted()
        {
            if (State == GameState.Open)
            {
                throw new DomainException(ErrorCode.GameNotActive, "Game is not started");
            }
        }
    }
}
