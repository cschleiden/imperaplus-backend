using ImperaPlus.Domain;

namespace ImperaPlus.DataAccess
{
    public interface IUserProvider
    {
        /// <summary>
        /// Gets the id of the current user
        /// </summary>
        /// <returns>User Id</returns>
        string GetCurrentUserId();
    }
}