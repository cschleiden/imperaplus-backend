namespace ImperaPlus.DTO.Account
{
    /// <summary>
    /// Information for specific user
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Global unique user identifier
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Value indicating whether this user is registered
        /// </summary>
        public bool HasRegistered { get; set; }

        /// <summary>
        /// LoginProvider used
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Language set for user profile
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Roles the user is currently in
        /// </summary>
        public string[] Roles { get; set; }
    }
}
