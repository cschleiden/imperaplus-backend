using System;

namespace ImperaPlus.DTO.Games
{
    [Flags]
    public enum CountryFlags
    {
        /// <summary>
        /// Default
        /// </summary>
        None = 0,

        /// <summary>
        /// Country is a capital of a player
        /// </summary>
        Capital = 1
    }
}
