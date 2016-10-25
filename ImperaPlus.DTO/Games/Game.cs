using System;

namespace ImperaPlus.DTO.Games
{
    /// <summary>
    /// Detailed information about a single game
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Identifier for game
        /// </summary>
        public int Id { get; set; }        

        /// <summary>
        /// Type of the game
        /// </summary>
        public GameType Type { get; set;}

        /// <summary>
        /// Name of the game
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the MapTemplate for this game
        /// </summary>
        public string MapTemplate { get; set; }

        /// <summary>
        /// List of teams
        /// </summary>
        public Team[] Teams { get; set; }

        /// <summary>
        /// Current state of the game
        /// </summary>
        public GameState State { get; set; }

        /// <summary>
        /// Current state of the turn
        /// </summary>
        public PlayState PlayState { get; set; }

        /// <summary>
        /// Imformation about current player
        /// </summary>
        public PlayerSummary CurrentPlayer { get; set; }

        /// <summary>
        /// Current state of the map
        /// </summary>
        public Map.Map Map { get; set; }

        /// <summary>
        /// Options set for this game
        /// </summary>
        public GameOptions Options { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastModifiedAt { get; set; }

        /// <summary>
        /// Number of seconds left until timeout is triggered
        /// </summary>
        public int TimeoutSecondsLeft { get; set; }

        /// <summary>
        /// Current turn
        /// </summary>
        public int TurnCounter { get; set; }

        /// <summary>
        /// Numbers of units to place for current player
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
    }
}