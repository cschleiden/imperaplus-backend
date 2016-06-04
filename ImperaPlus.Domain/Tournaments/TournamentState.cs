namespace ImperaPlus.Domain.Tournaments
{
    public enum TournamentState
    {
        /// <summary>
        /// Tournament is open for new players
        /// </summary>
        Open = 0,

        /// <summary>
        /// Tournament is in the Group phase
        /// </summary>
        Groups = 1,

        /// <summary>
        /// Tournament is in the Knockout phase
        /// </summary>
        Knockout = 2,

        /// <summary>
        /// Tournament has been closed
        /// </summary>
        Closed = 3
    }
}