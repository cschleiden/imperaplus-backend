namespace ImperaPlus.DTO.Tournaments
{
    public enum TournamentTeamState
    {
        /// <summary>
        /// Team is open, members can join/leave
        /// </summary>
        Open = 0,

        /// <summary>
        /// Team is active in the tournament
        /// </summary>
        Active = 1,

        /// <summary>
        /// Team is not active in the tournament anymore
        /// </summary>
        InActive = 2
    }
}
