namespace ImperaPlus.DTO.Users
{
    /// <summary>
    /// Shallow reference to a user
    /// </summary>
    public class UserReference
    {
        /// <summary>
        /// Unique user identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }
    }
}
