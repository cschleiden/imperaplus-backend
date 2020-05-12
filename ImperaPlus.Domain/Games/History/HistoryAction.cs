namespace ImperaPlus.Domain.Games.History
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
        /// Player has a timeout
        /// </summary>
        PlayerTimeout = 9,

        /// <summary>
        /// Countries changes owner, e.g., because a player quit
        /// </summary>
        OwnerChange = 10,

        /// <summary>
        /// Turn has ended
        /// </summary>
        EndTurn = 11,

        /// <summary>
        /// Player has surrendered
        /// </summary>
        PlayerSurrender = 12,

        /// <summary>
        /// Capital was lost
        /// </summary
        CapitalLost = 13,
    }
}