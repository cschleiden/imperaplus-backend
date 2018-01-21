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
    public class Game : Entity, IIdentifiableEntity, IChangeTrackedEntity, IOwnedEntity, ISerializedEntity
    {
        [UsedImplicitly]
        protected Game()
        {
            this.ChatMessages = new HashSet<GameChatMessage>();
            this.HistoryEntries = new List<HistoryEntry>();
            this.GameHistory = new GameHistory(this);
            this.Countries = new SerializedCollection<Country>();
            this.Teams = new HashSet<Team>();
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
            : this(user, type, name, password, mapTemplateName, new GameOptions
            {
                NumberOfPlayersPerTeam = numberOfPlayersPerTeam,
                NumberOfTeams = numberOfTeams,
                TimeoutInSeconds = timeoutInSeconds
            })
        {
            this.Options.VictoryConditions.AddRange(victoryConditions);
            this.Options.VisibilityModifier.AddRange(visibilityModifier);
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
            this.Type = type;
            this.Name = name;
            this.Password = password;

            if (this.Password != null && this.Type != GameType.Fun)
            {
                throw new DomainException(ErrorCode.GamePasswordOnlyAllowedForFun, "Passwords for games are only allowed for fun games");
            }

            this.CreatedBy = user;
            this.CreatedAt = DateTime.UtcNow;

            this.MapTemplateName = mapTemplateName;

            this.Options = options;

            this.State = GameState.Open;
            this.PlayState = PlayState.None;
        }

        public void Serialize()
        {
            this.countriesSerialized = this.Countries.Serialize();
        }

        /// <summary>
        /// Has to have a different name than the property, want to force EF to use the property
        /// </summary>
        private string countriesSerialized;
        public string SerializedCountries
        {
            get
            {
                return this.countriesSerialized;
            }

            set
            {
                this.countriesSerialized = value;
                this.Countries = new SerializedCollection<Country>(this.countriesSerialized);
            }
        }

        public long Id { get; set; }

        /// <summary>
        /// Used for concurrency checks
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }

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

        /// <summary>
        /// Use the property
        /// </summary>
        private Map map;

        [NotMapped]
        public Map Map
        {
            get
            {
                if (this.map == null)
                {
                    this.map = new Map(this, this.Countries);
                }

                return this.map;
            }
        }

        [NotMapped]
        public SerializedCollection<Country> Countries { get; private set; }

        public virtual ICollection<Team> Teams { get; private set; }

        [NotMapped]
        public GameHistory GameHistory { get; private set; }

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
                if (!this.CurrentPlayerId.HasValue)
                {
                    return null;
                }

                return this.Teams.SelectMany(x => x.Players).FirstOrDefault(p => p.Id == this.CurrentPlayerId.Value);
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
                var players = this.Teams.SelectMany(x => x.Players).Count();

                return players == this.Options.PlayerCount && this.State == GameState.Open;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the game can be deleted
        /// </summary>
        public bool CanBeDeleted
        {
            get
            {
                return this.State == GameState.Open;
            }
        }

        /// <summary>
        /// Gets a value indicating whether players can leave the game
        /// </summary>
        [NotMapped]
        public bool CanLeave
        {
            get
            {
                return this.State == GameState.Open;
            }
        }

        /// <summary>
        /// Adds a new team
        /// </summary>
        /// <returns>New team</returns>
        public Team AddTeam()
        {
            if (this.Teams.Count() >= this.Options.NumberOfTeams)
            {
                throw new DomainException(ErrorCode.TooManyTeams, "Cannot create team, there are too many teams already");
            }

            var team = new Team(this);

            this.Teams.Add(team);

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

            this.RequireGameStarted();

            var player = this.GetPlayerForUser(user.Id);

            Team team = null;
            if (!isPublic)
            {
                team = player.Team;
            }

            var message = new GameChatMessage(this, user, team, text);

            this.ChatMessages.Add(message);

            return message;
        }

        public IEnumerable<GameChatMessage> GetMessages(User user, bool isPublic)
        {
            Require.NotNull(user, "user");

            var player = this.GetPlayerForUser(user.Id);

            IEnumerable<GameChatMessage> messages;
            if (isPublic)
            {
                messages = this.ChatMessages.Where(x => x.TeamId == null);
            }
            else
            {
                messages = this.ChatMessages.Where(x => x.TeamId == player.TeamId);
            }

            return messages.OrderBy(x => x.TeamId).OrderBy(x => x.DateTime).ToList();
        }

        /// <summary>
        /// Add player and implicitly create team
        /// </summary>
        public Player AddPlayer(User user, string password = null)
        {
            Require.NotNull(user, "user");

            if (this.IsPasswordProtected && !string.Equals(password, this.Password, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new DomainException(ErrorCode.GamePasswordNotCorrect, "Game is password protected and given password does not match");
            }

            if (this.Teams.SelectMany(x => x.Players).Any(x => x.UserId == user.Id))
            {
                throw new DomainException(ErrorCode.PlayerAlreadyJoined, "Player has already joined this game.");
            }

            Team team;
            if (this.Teams.Count < this.Options.NumberOfTeams)
            {
                team = this.AddTeam();
            }
            else
            {
                team = this.Teams.FirstOrDefault(x => x.Players.Count() < this.Options.NumberOfPlayersPerTeam);
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

            if (!this.CanLeave)
            {
                throw new DomainException(ErrorCode.CannotLeaveGame, "Game state does not allow leaving");
            }

            if (user.Id == this.CreatedBy.Id)
            {
                throw new DomainException(ErrorCode.CannotLeaveGame, "Game creator cannot leave game");
            }

            var player = this.GetPlayerForUser(user.Id);
            var team = player.Team;

            team.RemovePlayer(player);

            if (!team.Players.Any())
            {
                // Remove empty team
                this.Teams.Remove(team);
            }
        }

        /// <summary>
        /// Start the game
        /// </summary>
        public void Start(MapTemplate mapTemplate, IRandomGen random)
        {
            if (!this.CanStart)
            {
                throw new DomainException(ErrorCode.CannotStartGame, "Cannot start");
            }

            this.StartedAt = DateTime.UtcNow;
            this.State = GameState.Active;

            TraceContext.Trace("Create Map from Template", () => Map.CreateFromTemplate(this, mapTemplate));
            TraceContext.Trace("Distribute countries to teams", () => this.Map.Distribute(this.Teams, mapTemplate, this.Options.MapDistribution, random));

            // Determine player order
            this.DeterminePlayOrder(random);

            // Set current player
            var currentTeam = this.Teams.RandomElement(random);
            var currentPlayer = currentTeam.Players.First();

            this.CurrentPlayerId = currentPlayer.Id;

            // Record in history
            this.GameHistory.RecordStart();

            this.TurnCounter = 1;

            this.ResetTurn();

            this.EventQueue.Raise(new GameStartedEvent(this));
        }

        /// <summary>
        /// Ends the current turn and switches to the next player
        /// </summary>
        public void EndTurn()
        {
            if (this.State != GameState.Active)
            {
                return;
            }

            this.GameHistory.RecordEndTurn();
            this.TurnCounter++;

            // Go to next player
            Player nextPlayer = null;
            for (int i = 1; i < 2 * this.Options.PlayerCount && nextPlayer == null && nextPlayer != this.CurrentPlayer; ++i)
            {
                var nextPlayerOrder = (this.CurrentPlayer.PlayOrder + i) % this.Options.PlayerCount;
                nextPlayer = this.Teams
                                    .SelectMany(x => x.Players)
                                    .OrderBy(x => x.PlayOrder)
                                    .FirstOrDefault(x => x.State == PlayerState.Active && x.PlayOrder >= nextPlayerOrder);
            }

            if (nextPlayer == null)
            {
                Log.Fatal().Message("Cannot find active player for game {0}", this.Id).Write();
                throw new DomainException(ErrorCode.GenericError, "Cannot find active player");
            }

            this.CurrentPlayerId = nextPlayer.Id;

            this.ResetTurn();
            this.EventQueue.Raise(new TurnEndedEvent(this));
        }

        private void ResetTurn()
        {
            this.CurrentPlayer.Bonus = 0;

            this.AttacksInCurrentTurn = 0;
            this.MovesInCurrentTurn = 0;

            this.CardDistributed = false;

            this.PlayState = PlayState.PlaceUnits;

            this.LastTurnStartedAt = DateTime.UtcNow;
        }

        public int GetUnitsToPlace(MapTemplate mapTemplate, Player player)
        {
            // Base units as configured
            var unitsToPlace = this.Options.NewUnitsPerTurn;

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
            this.RequireGameActive();

            if (this.PlayState != PlayState.PlaceUnits)
            {
                throw new DomainException(ErrorCode.ExchangingCardsNotAllowed, "Exchanging cards is not allowed");
            }

            var unitsReceived = this.CurrentPlayer.ExchangeCards();
            if (unitsReceived > 0)
            {
                this.GameHistory.RecordCardExchange(this.CurrentPlayer, this.CurrentPlayer.Bonus);
            }
        }

        public void PlaceUnits(MapTemplate mapTemplate, IEnumerable<Tuple<string, int>> countries)
        {
            this.RequireGameActive();

            if (this.PlayState != PlayState.PlaceUnits)
            {
                throw new DomainException(ErrorCode.PlacingNotAllowed, "Placing units is not allowed");
            }

            var unitsToPlace = countries.Sum(x => x.Item2);
            var unitsAvailable = this.GetUnitsToPlace(mapTemplate, this.CurrentPlayer);
            if (unitsToPlace > unitsAvailable)
            {
                throw new DomainException(ErrorCode.PlacingMoreUnitsThanAvailable, "Cannot place more units than available");
            }
            else if (unitsToPlace < unitsAvailable)
            {
                throw new DomainException(ErrorCode.PlacingLessUnitsThanAvailable, "Cannot place less units than available");
            }

            foreach (var place in countries)
            {
                var country = this.Map.GetCountry(place.Item1);

                // Check ownership
                var owner = this.GetPlayerById(country.PlayerId);
                if (owner.TeamId != this.CurrentPlayer.TeamId)
                {
                    throw new DomainException(ErrorCode.PlacingToForeignCountry, "Country does not belong to current team");
                }

                country.PlaceUnits(place.Item2);

                this.GameHistory.RecordPlace(this.CurrentPlayer, country.CountryIdentifier, place.Item2);
            }

            if (!this.CurrentPlayer.PlacedInitialUnits)
            {
                this.CurrentPlayer.PlacedInitialUnits = true;

                // In the first turn, players are only allowed to place
                this.EndTurn();
            }
            else
            {
                this.PlayState = PlayState.Attack;
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
            this.RequireGameActive();

            if (this.PlayState != PlayState.Attack)
            {
                throw new DomainException(ErrorCode.AttackingNotPossible, "Cannot attack, state incorrect");
            }

            var sourceCountry = this.Map.GetCountry(sourceCountryIdentifier);
            var destCountry = this.Map.GetCountry(destCountryIdentifier);

            // Check connection
            if (!mapTemplate.AreConnected(sourceCountryIdentifier, destCountryIdentifier))
            {
                throw new DomainException(ErrorCode.CountriesNotConnected, "There is no connection between those countries");
            }

            // Check ownership
            if (sourceCountry.TeamId != this.CurrentPlayer.TeamId)
            {
                throw new DomainException(ErrorCode.OriginCountryNotOwnedByTeam, "Can only initiate actions from countries that belong to the same team");
            }

            if (sourceCountry.PlayerId == destCountry.PlayerId)
            {
                throw new DomainException(ErrorCode.AttackOwnCountries, "Cannot attack own countries");
            }

            if (numberOfUnits <= 0 || sourceCountry.Units - numberOfUnits < this.Options.MinUnitsPerCountry)
            {
                throw new DomainException(ErrorCode.NotEnoughUnits, "Cannot attack with that many units");
            }

            var otherPlayer = this.GetPlayerById(destCountry.PlayerId);

            int attackerUnitsLost = 0;
            int defenderUnitsLost = 0;
            var result = attackService.Attack(numberOfUnits, destCountry.Units, out attackerUnitsLost, out defenderUnitsLost);

            if (result)
            {
                // Attack was successful
                destCountry.Units = numberOfUnits - attackerUnitsLost;

                this.Map.UpdateOwnership(otherPlayer, this.CurrentPlayer, destCountry);

                this.DistributeCard(randomGen);
            }
            else
            {
                // Attack failed
                destCountry.Units -= defenderUnitsLost;
            }

            // Reduce units in this country in any case
            sourceCountry.Units -= numberOfUnits;

            this.GameHistory.RecordAttack(
                this.CurrentPlayer, otherPlayer,
                sourceCountryIdentifier, destCountryIdentifier,
                numberOfUnits, attackerUnitsLost, defenderUnitsLost,
                result);

            // Reduce number of attacks left
            this.AttacksInCurrentTurn++;

            // Check for victory
            this.CheckForVictory(this.CurrentPlayer, otherPlayer);

            if (this.AttacksInCurrentTurn >= this.Options.AttacksPerTurn)
            {
                this.EndAttack();
            }
        }

        public void EndAttack()
        {
            this.PlayState = PlayState.Move;
        }

        private void DistributeCard(IRandomGen randomGen)
        {
            if (!this.CardDistributed)
            {
                this.CardDistributed = true;

                if (this.CurrentPlayer.Cards.Count() < this.Options.MaximumNumberOfCards)
                {
                    var existingCards = new List<BonusCard>(this.CurrentPlayer.Cards);

                    var cardToDistribute = randomGen.GetNext(0, 2);
                    existingCards.Add((BonusCard)cardToDistribute);

                    this.CurrentPlayer.Cards = existingCards.ToArray();
                }
            }
        }

        internal void CheckForVictory(Player active, Player passive = null)
        {
            foreach (var victoryCondition in this.Options.VictoryConditions)
            {
                var victoryConditionImpl = VictoryConditionFactory.Create(victoryCondition);

                // Check for passive player
                if (passive != null)
                {
                    if (victoryConditionImpl.Evaluate(passive, this.Map) == VictoryConditionResult.Defeat)
                    {
                        passive.Outcome = PlayerOutcome.Defeated;
                        passive.State = PlayerState.InActive;

                        this.GameHistory.RecordPlayerDefeated(passive);
                        // TODO: CS: Generate event
                    }
                }

                // Check for active player
                if (victoryConditionImpl.Evaluate(active, this.Map) == VictoryConditionResult.Victory)
                {
                    active.Outcome = PlayerOutcome.Won;
                    active.State = PlayerState.InActive;
                }
            }

            // Update team status
            var activeTeams = this.Teams.Where(x => x.Players.Any(p => !p.HasLost));
            if (activeTeams.Count() == 1)
            {
                // Only one team has survived, all players in this team have won
                foreach (var player in activeTeams.Single().Players)
                {
                    player.Outcome = PlayerOutcome.Won;
                    player.State = PlayerState.InActive;

                    this.GameHistory.RecordPlayerWon(player);
                    // TODO: CS: Generate event
                }

                this.End();
            }
        }

        public void End()
        {
            // Set this first to prevent EndTurn from determining new player
            this.State = GameState.Ended;

            this.EndTurn();

            this.GameHistory.RecordEnd();

            this.EventQueue.Raise(new GameEndedEvent(this));
        }

        public void ResetTracking()
        {
            this.Map.ResetTracking();
        }

        public Player GetPlayerForUser(string userId)
        {
            var player = this.Teams.SelectMany(x => x.Players).FirstOrDefault(x => x.UserId == userId);

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

            var player = this.Teams.SelectMany(x => x.Players).FirstOrDefault(x => x.Id == id);
            if (null == player)
            {
                throw new DomainException(ErrorCode.PlayerNotInGame, "Cannot find player with id");
            }

            return player;
        }

        public void ProcessTimeouts()
        {
            if (DateTime.UtcNow - this.LastTurnStartedAt > TimeSpan.FromSeconds(this.Options.TimeoutInSeconds))
            {
                ++this.CurrentPlayer.Timeouts;
                this.GameHistory.RecordTimeout();

                if (this.Options.MaximumTimeoutsPerPlayer > 0
                    && this.CurrentPlayer.Timeouts > this.Options.MaximumTimeoutsPerPlayer)
                {
                    // TODO: CS: Refactor?
                    this.CurrentPlayer.State = PlayerState.InActive;
                    this.CurrentPlayer.Outcome = PlayerOutcome.Defeated;

                    foreach (var country in this.CurrentPlayer.Countries.ToArray())
                    {
                        this.Map.UpdateOwnership(null, country);
                        this.GameHistory.RecordOwnershipChange(this.CurrentPlayer, null, country.CountryIdentifier);
                    }

                    this.CheckForVictory(this.CurrentPlayer);
                }

                this.EndTurn();
            }
        }

        /// <summary>
        /// Gets the number of slots this game occupies for each player
        /// </summary>
        public int RequiredSlots
        {
            get { return 1 /* + MapSlots*/; }
        }

        public bool IsPasswordProtected
        {
            get
            {
                return this.Password != null;
            }
        }

        public void Move(
            MapTemplate mapTemplate,
            string sourceCountryIdentifier,
            string destCountryIdentifier,
            int numberOfUnits)
        {
            this.RequireGameActive();

            // Check connection
            if (!mapTemplate.AreConnected(sourceCountryIdentifier, destCountryIdentifier))
            {
                throw new DomainException(ErrorCode.CountriesNotConnected, "Countries are not connected");
            }

            var sourceCountry = this.Map.GetCountry(sourceCountryIdentifier);
            var destCountry = this.Map.GetCountry(destCountryIdentifier);

            if (numberOfUnits <= 0 || sourceCountry.Units - numberOfUnits < this.Options.MinUnitsPerCountry)
            {
                throw new DomainException(ErrorCode.NotEnoughUnits, "Cannot move that many units");
            }

            // Check ownership
            if (sourceCountry.TeamId != this.CurrentPlayer.TeamId)
            {
                throw new DomainException(ErrorCode.OriginCountryNotOwnedByTeam, "Can only initiate actions from countries that belong to the same team");
            }

            if (sourceCountry.TeamId != destCountry.TeamId)
            {
                throw new DomainException(ErrorCode.MoveOwnCountries, "Units can only be moved between countries that belong to the same team");
            }

            if (this.PlayState != PlayState.Move)
            {
                if (this.PlayState == PlayState.Attack)
                {
                    // Switch automatically to move and end attacking
                    this.PlayState = PlayState.Move;
                }
                else
                {
                    throw new DomainException(ErrorCode.MovingNotPossible, "Cannot move, state incorrect");
                }
            }

            // Perform move
            sourceCountry.Units -= numberOfUnits;
            destCountry.Units += numberOfUnits;

            this.GameHistory.RecordMove(
                this.CurrentPlayer,
                sourceCountry.CountryIdentifier, destCountry.CountryIdentifier, numberOfUnits);

            this.MovesInCurrentTurn++;
            if (this.MovesInCurrentTurn >= this.Options.MovesPerTurn)
            {
                this.PlayState = PlayState.Done;
            }
        }

        private void DeterminePlayOrder(IRandomGen random)
        {
            // Desired outcome: 
            // t1: p1 - 0
            // t2: p1 - 1
            // t1: p2 - 2
            // t2: p2 - 3

            var shuffledTeams = this.Teams.Shuffle(random).ToArray();
            int teamIdx = 0;
            foreach (var shuffledTeam in shuffledTeams)
            {
                shuffledTeam.PlayOrder = teamIdx;

                int playerIdx = 0;
                foreach (var player in shuffledTeam.Players)
                {
                    player.PlayOrder = teamIdx + playerIdx * this.Options.NumberOfTeams;

                    ++playerIdx;
                }

                ++teamIdx;
            }
        }

        private void RequireGameActive()
        {
            if (this.State != GameState.Active)
            {
                throw new DomainException(ErrorCode.GameNotActive, "Game is not active");
            }
        }

        private void RequireGameStarted()
        {
            if (this.State == GameState.Open)
            {
                throw new DomainException(ErrorCode.GameNotActive, "Game is not started");
            }
        }
    }
}
