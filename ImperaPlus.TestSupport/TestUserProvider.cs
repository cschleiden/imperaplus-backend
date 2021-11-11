using ImperaPlus.Domain;

namespace ImperaPlus.TestSupport
{
    public class TestUserProvider : IUserProvider
    {
        public static User User { get; set; }

        public static bool Admin { get; set; }

        public string GetCurrentUserId()
        {
            return User.Id;
        }

        public bool IsAdmin()
        {
            return Admin;
        }

        public TestUserProvider()
        {
            // Reset
            User = null;
            Admin = false;
        }
    }
}
