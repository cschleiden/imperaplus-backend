using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Users;

namespace ImperaPlus.Domain.Messages.EventHandler
{
    public class AccountDeletedHandler : IEventHandler<AccountDeleted>
    {
        private IUnitOfWork unitOfWork;

        public AccountDeletedHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Handle(AccountDeleted evt)
        {
            var messages = this.unitOfWork.Messages.OwnedByUser(evt.User.Id);
            foreach(var message in messages)
            {
                this.unitOfWork.Messages.Remove(message);
            }
        }
    }
}
