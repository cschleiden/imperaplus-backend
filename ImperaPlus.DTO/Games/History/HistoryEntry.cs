using System;

namespace ImperaPlus.DTO.Games.History
{
    public enum HistoryAction
    {
        None = 0,

        /// <summary>
        /// Game has started
        /// </summary>
        StartGame = 1,

        /// <summary>
        /// Game has ended
        /// </summary>
        EndGame = 2,

        /// <summary>
        /// Player has placed units
        /// </summary>
        PlaceUnits = 3,

        /// <summary>
        /// Player has attacked
        /// </summary>
        Attack = 4,

        /// <summary>
        /// Player has moved units
        /// </summary>
        Move = 5,

        /// <summary>
        /// Player has exchanged cards
        /// </summary>
        ExchangeCards = 6,

        /// <summary>
        /// Player has lost the game
        /// </summary>
        PlayerLost = 7,

        /// <summary>
        /// Player has won the game
        /// </summary>
        PlayerWon = 8,

        /// <summary>
        /// Player has run into a timeout
        /// </summary>
        PlayerTimeout = 9,

        /// <summary>
        /// Player has surrendered
        /// </summary>
        PlayerSurrendered = 12,

        /// <summary>
        /// Countries changes owner, e.g., because a player quit
        /// </summary>
        OwnerChange = 10,

        /// <summary>
        /// Turn has ended
        /// TODO: CS: Required?
        /// </summary>
        EndTurn = 11
    }

    /// <summary>
    /// Represents a single action in the game history
    /// </summary>
    public class HistoryEntry
    {               
        /// <summary>
        /// Unique id of this history entry
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Number of turn this entry belongs to
        /// </summary>
        public long TurnNo { get; set; }

        /// <summary>
        /// Date and time the action was performed
        /// </summary>
        public DateTime DateTime { get; set; }
        
        /// <summary>
        /// Id of acting player
        /// </summary>
        public Guid ActorId { get; set; }

        /// <summary>
        /// Id of other player involved (e.g., when the entry depicts an attack)
        /// </summary>
        public Guid? OtherPlayerId { get; set; }

        /// <summary>
        /// Depicts the action of this entry
        /// </summary>
        public HistoryAction Action { get; private set; }

        /// <summary>
        /// Identifier of the originating country (if applicable to action)
        /// </summary>
        public string OriginIdentifier { get; set; }

        /// <summary>
        /// Identifier of the destination country (if applicable to action)
        /// </summary>
        public string DestinationIdentifier { get; set; }

        /// <summary>
        /// Number of units (if applicable to action)
        /// </summary>
        public int? Units { get; set; }

        /// <summary>
        /// Number of units lost (applicable to attack only)
        /// </summary>
        public int? UnitsLost { get; set; }

        /// <summary>
        /// Number of units other player has lost (applicable to attack only)
        /// </summary>
        public int? UnitsLostOther { get; set; }

        /// <summary>
        /// Result of this action (applicable to attack only, depicts if it was successful)
        /// </summary>
        public bool? Result { get; set; }
    }

    /// <summary>
    /// Information about a completed turn
    /// </summary>
    public class HistoryTurn
    {
        /// <summary>
        /// Id of game
        /// </summary>
        public long GameId { get; set; }

        /// <summary>
        /// Id of turn
        /// </summary>
        public long TurnId { get; set; }

        /// <summary>
        /// List of actions in the turn
        /// </summary>
        public HistoryEntry[] Actions { get; set; }

        /// <summary>
        /// State of the game at the end of the turn
        /// </summary>
        public Game Game { get; set; }
    }
}
