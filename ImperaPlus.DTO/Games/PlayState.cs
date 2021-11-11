namespace ImperaPlus.DTO.Games
{
    /// <summary>
    /// State of a turn
    /// </summary>
    public enum PlayState
    {
        /// <summary>
        /// Default
        /// </summary>
        None,

        /// <summary>
        /// Player can place units and exchange cards
        /// </summary>
        PlaceUnits,

        /// <summary>
        /// Player can attack
        /// </summary>
        Attack,

        /// <summary>
        /// Player can move units
        /// </summary>
        Move,

        /// <summary>
        /// Player has exhausted all actions
        /// </summary>
        Done
    }
}
