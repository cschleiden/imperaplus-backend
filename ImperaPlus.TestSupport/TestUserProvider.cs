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

        public bool IsAdmin()
        {
            return Admin;
        }

        public static User User { get; set; }

        public static bool Admin { get; set; }
    }
}