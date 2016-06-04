using System;

namespace ImperaPlus.Domain.Games.History
{
    public class HistoryEntry : IIdentifiableEntity
    {
        private HistoryEntry()
        {

        }

        public HistoryEntry(Game game, Player player, HistoryAction action, long turnNo)
        {
            this.GameId = game.Id;
            this.Game = game;

            if (player != null)
            {
                this.ActorId = player.Id;
                this.Actor = player;
            }

            this.Action = action;
            this.TurnNo = turnNo;

            this.DateTime = DateTime.UtcNow;
        }

        public long Id { get; set; }

        public long TurnNo { get; set; }

        public DateTime DateTime { get; set; }

        public long GameId { get; set; }
        public virtual Game Game { get; private set; }

        public Guid? ActorId { get; set; }
        public virtual Player Actor { get; private set; }

        public Guid? OtherPlayerId { get; set; }
        public virtual Player OtherPlayer { get; set; }

        public HistoryAction Action { get; private set; }

        public string OriginIdentifier { get; set; }

        public string DestinationIdentifier { get; set; }

        public int? Units { get; set; }

        public int? UnitsLost { get; set; }

        public int? UnitsLostOther { get; set; }

        public bool? Result { get; set; }

        // TODO: CS: What about other country properties here?
    }
}