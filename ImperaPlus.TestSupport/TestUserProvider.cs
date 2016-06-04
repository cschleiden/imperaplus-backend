using ImperaPlus.DataAccess;
using ImperaPlus.Domain;

namespace ImperaPlus.TestSupport
{
    public class TestUserProvider : IUserProvider
    {
        public string GetCurrentUserId()
        {
            return User.Id;
        }

        public User GetCurrentUser()
        {
            return User;
        }

        public static User User { get; set; }
    }
}