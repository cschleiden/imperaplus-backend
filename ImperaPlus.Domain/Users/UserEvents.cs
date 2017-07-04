using ImperaPlus.Domain.Events;

namespace ImperaPlus.Domain.Users
{
    public class AccountDeleted : IDomainEvent
    {
        public AccountDeleted(User user)
        {
            this.User = user;
        }

        public User User { get; private set; }
    }
}
