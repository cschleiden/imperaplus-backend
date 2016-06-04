namespace ImperaPlus.DTO.Games
{
    /// <summary>
    /// Visibility modifier
    /// </summary>
    public enum VisibilityModifierType
    {
        /// <summary>
        /// Players see the complete map
        /// </summary>
        None = 0,

        /// <summary>
        /// Players only see their own countries and connected ones
        /// </summary>
        Fog = 1
    }
}
