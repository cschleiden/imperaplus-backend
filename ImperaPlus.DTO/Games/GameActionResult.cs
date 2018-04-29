namespace ImperaPlus.DTO.Games
{
    /// <summary>
    /// 
    /// </summary>
    public class GameActionResult
    {
        /// <summary>
        /// Id of game
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Turn counter
        /// </summary>
        public int TurnCounter { get; set; }

        /// <summary>
        /// List of teams
        /// </summary>
        public Team[] Teams { get; set; }

        /// <summary>
        /// Current game states
        /// </summary>
        public GameState State { get; set; }

        /// <summary>
        /// Current state of turn
        /// </summary>
        public PlayState PlayState { get; set; }

        /// <summary>
        /// Updates for changed countries
        /// </summary>
        public Map.Country[] CountryUpdates { get; set; }

        /// <summary>
        /// Result of the previous action
        /// </summary>
        public Result ActionResult { get; set; }

        /// <summary>
        /// Units to place
        /// </summary>
        public int UnitsToPlace { get; set; }

        /// <summary>
        /// Numbers of attacks done in current turn
        /// </summary>
        public int AttacksInCurrentTurn { get; set; }

        /// <summary>
        /// Numbers of moves done in current turn
        /// </summary>
        public int MovesInCurrentTurn { get; set; }

        /// <summary>
        /// Comma separated list of bonus cards
        /// </summary>
        public BonusCard[] Cards { get; set; }

        /// <summary>
        /// During an action the current player might have changed
        /// </summary>
        public Player CurrentPlayer { get; set; }
    }
}
