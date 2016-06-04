namespace ImperaPlus.DTO.Ladder
{
    /// <summary>
    /// Single ladder
    /// </summary>
    public class Ladder : LadderSummary
    {
        /// <summary>
        /// All standings for ladder
        /// </summary>
        public LadderStanding[] Standings { get; set; }

        /// <summary>
        /// Value indicating whether ladder is active
        /// </summary>
        public bool IsActive { get; set; }
    }
}
