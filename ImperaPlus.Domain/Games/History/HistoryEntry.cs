using System;
using ImperaPlus.Domain.Enums;

namespace ImperaPlus.Domain.Games.History
{
    public class HistoryEntry : IIdentifiableEntity
    {
        protected HistoryEntry()
        {
        }

        public HistoryEntry(Game game, Player player, HistoryAction action, long turnNo)
        {
            GameId = game.Id;
            Game = game;

            if (player != null)
            {
                ActorId = player.Id;
                Actor = player;
            }

            Action = action;
            TurnNo = turnNo;

            DateTime = DateTime.UtcNow;
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
    }
}
