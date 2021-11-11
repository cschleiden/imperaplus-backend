using ImperaPlus.Domain.Events;

namespace ImperaPlus.Domain.Users
{
    public class AccountDeleted : IDomainEvent
    {
        public AccountDeleted(User user, bool force = false)
        {
            User = user;
            Force = force;
        }

        public User User { get; private set; }

        public bool Force { get; private set; }
    }
}
