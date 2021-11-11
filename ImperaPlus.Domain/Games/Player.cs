using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Games.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ImperaPlus.Domain.Games
{
    public class Player : Entity
    {
        protected Player()
        {
            Id = Guid.NewGuid();

            InternalCardData = string.Empty;
            Timeouts = 0;
        }

        public Player(Game game, User user, Team team)
            : this()
        {
            Game = game;
            GameId = game.Id;

            User = user;
            UserId = user.Id;

            Team = team;
            TeamId = team.Id;

            Bonus = 0;
            State = PlayerState.Active;
            Outcome = PlayerOutcome.None;

            PlayOrder = team.Game.Teams.SelectMany(x => x.Players).Count();

            IsHidden = false;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public virtual Game Game { get; set; }
        public long? GameId { get; set; }

        public string UserId { get; private set; }
        public virtual User User { get; private set; }

        public Guid TeamId { get; internal set; }
        public virtual Team Team { get; internal set; }

        public int PlayOrder { get; set; }

        /// <summary>
        /// Number of timeouts for this user
        /// </summary>
        public int Timeouts { get; set; }

        /// <summary>
        /// Indicates whether this game is hidden for this player
        /// </summary>
        public bool IsHidden { get; private set; }

        /// <summary>
        /// Indicates whether player has placed units in his first turn
        /// </summary>
        public bool PlacedInitialUnits { get; internal set; }

        [NotMapped]
        public BonusCard[] Cards
        {
            get => ParseCardData(InternalCardData);

            set => InternalCardData = string.Join(";", value);
        }

        /// <summary>
        /// Holds bonus units exchanged for cards
        /// </summary>
        public int Bonus { get; set; }

        /// <summary>
        /// Used for storing, use Cards to acess
        /// </summary>
        public string InternalCardData { get; set; }

        public PlayerState State { get; set; }

        public PlayerOutcome Outcome { get; set; }

        [NotMapped] public IEnumerable<Country> Countries => Team.Game.Map.GetCountriesForPlayer(this);

        public bool HasLost =>
            Outcome == PlayerOutcome.Defeated
            || Outcome == PlayerOutcome.Surrendered
            || Outcome == PlayerOutcome.Timeout;

        public static BonusCard[] ParseCardData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return new BonusCard[0];
            }

            return data
                .Split(';')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(source => (BonusCard)Enum.Parse(typeof(BonusCard), source))
                .ToArray();
        }

        internal int ExchangeCards()
        {
            var bonusUnits = 0;

            var cards = Cards;
            Array.Sort(cards);

            var counts = new List<int>();

            counts.Add(cards.Count(x => x == BonusCard.A));
            counts.Add(cards.Count(x => x == BonusCard.B));
            counts.Add(cards.Count(x => x == BonusCard.C));

            var tripleBonusCount = counts.Min();
            if (tripleBonusCount > 0)
            {
                bonusUnits += tripleBonusCount * 5;
            }

            var newCards = new List<BonusCard>();

            for (var i = 0; i < counts.Count(); ++i)
            {
                var count = counts[i] - tripleBonusCount;

                while (count >= 3)
                {
                    count -= 3;
                    bonusUnits += 3;
                }

                counts[i] = count;

                newCards.AddRange(Enumerable.Repeat((BonusCard)i, count));
            }

            Cards = newCards.ToArray();
            Bonus += bonusUnits;

            return bonusUnits;
        }

        public void Hide()
        {
            if (Team.Game.State == GameState.Open || State != PlayerState.InActive)
            {
                throw new DomainException(ErrorCode.CannotHideGame, "Cannot hide game for player");
            }

            IsHidden = true;
        }

        /// <summary>
        /// Surrender in game
        /// </summary>
        public void Surrender()
        {
            if (Outcome != PlayerOutcome.None)
            {
                throw new DomainException(ErrorCode.CannotSurrender, "Player is not active");
            }

            var game = Team.Game;
            if (game.State != GameState.Active)
            {
                throw new DomainException(ErrorCode.GameNotActive, "Can only surrender if game is active");
            }

            Outcome = PlayerOutcome.Surrendered;
            State = PlayerState.InActive;

            ForfeitCountries();

            Team.Game.GameHistory.RecordPlayerSurrendered(this);
            Team.Game.CheckForVictory();

            EventQueue.Raise(new PlayerSurrenderedEvent(Team.Game, this));

            if (Game.CurrentPlayer == this)
            {
                // Player was the current player, advance turn
                Game.EndTurn();
            }
        }

        public void ForfeitCountries()
        {
            var game = Team.Game;

            // Change player's countries to neutral player
            foreach (var country in Countries.ToArray())
            {
                game.Map.UpdateOwnership(this, null, country);
                game.GameHistory.RecordOwnershipChange(this, null, country.CountryIdentifier);
            }
        }
    }
}
