using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Games.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ImperaPlus.Domain.Events;

namespace ImperaPlus.Domain.Games
{
    public class Player : Entity
    {
        private Player()
        {
            this.Id = Guid.NewGuid();

            this.InternalCardData = string.Empty;
            this.Timeouts = 0;
        }

        public Player(User user, Team team)
            : this()
        {
            this.User = user;
            this.UserId = user.Id;

            this.Team = team;
            this.TeamId = team.Id;

            this.Bonus = 0;
            this.State = PlayerState.Active;
            this.Outcome = PlayerOutcome.None;

            this.PlayOrder = team.Game.Teams.SelectMany(x => x.Players).Count();

            this.IsHidden = false;
        }

        public Guid Id { get; set; }

        public string UserId { get; private set; }
        public User User { get; private set; }

        public Guid TeamId { get; internal set; }
        public Team Team { get; internal set; }

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
            get
            {
                return ParseCardData(this.InternalCardData);
            }

            set
            {
                this.InternalCardData = string.Join(";", value);
            }
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

        [NotMapped]
        public IEnumerable<Country> Countries
        {
            get
            {
                return this.Team.Game.Map.GetCountriesForPlayer(this);
            }
        }

        public bool HasLost
        {
            get
            {
                return this.Outcome == PlayerOutcome.Defeated
                    || this.Outcome == PlayerOutcome.Surrendered
                    || this.Outcome == PlayerOutcome.Timeout;
            }
        }

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

            var cards = this.Cards;
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

            for (int i = 0; i < counts.Count(); ++i)
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

            this.Cards = newCards.ToArray();
            this.Bonus += bonusUnits;

            return bonusUnits;
        }

        public void Hide()
        {
            if (this.Team.Game.State == GameState.Open || this.State != PlayerState.InActive)
            {
                throw new DomainException(ErrorCode.CannotHideGame, "Cannot hide game for player");
            }

            this.IsHidden = true;
        }

        /// <summary>
        /// Surrender in game
        /// </summary>
        public void Surrender()
        {
            if (this.Outcome != PlayerOutcome.None)
            {
                throw new DomainException(ErrorCode.CannotSurrender, "Player is not active");
            }

            this.Outcome = PlayerOutcome.Surrendered;
            this.State = PlayerState.InActive;

            foreach (var country in this.Countries.ToArray())
            {
                this.Team.Game.Map.UpdateOwnership(this, null, country);
            }

            // TODO: CS: Record in history?
            this.Team.Game.CheckForVictory(this.Team.Game.CurrentPlayer);

            this.EventQueue.Raise(new PlayerSurrenderedEvent(this.Team.Game, this));
        }        
    }
}
