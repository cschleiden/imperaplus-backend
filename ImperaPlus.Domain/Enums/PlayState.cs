namespace ImperaPlus.Domain.Enums
{
    public enum PlayState
    {
        /// <summary>
        /// Default
        /// </summary>
        None = 0,

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
