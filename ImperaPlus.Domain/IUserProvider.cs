using ImperaPlus.Domain;

namespace ImperaPlus.Domain
{
    public interface IUserProvider
    {
        /// <summary>
        /// Gets the id of the current user
        /// </summary>
        /// <returns>User Id</returns>
        string GetCurrentUserId();

        /// <summary>
        /// Gets whether the current user is an admin
        /// </summary>
        /// <returns></returns>
        bool IsAdmin();
    }
}