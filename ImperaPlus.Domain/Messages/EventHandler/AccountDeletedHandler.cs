using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Users;

namespace ImperaPlus.Domain.Messages.EventHandler
{
    public class AccountDeletedHandler : IEventHandler<AccountDeleted>
    {
        private readonly IUnitOfWork unitOfWork;

        public AccountDeletedHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Handle(AccountDeleted evt)
        {
            this.RemoveMessagesOwnedByUser(evt.User.Id);

            this.UpdateMessagesSentByUser(evt.User.Id);

            this.UpdateMessagesReceivedByUser(evt.User.Id);
        }

        private void RemoveMessagesOwnedByUser(string userId)
        {
            var messages = this.unitOfWork.Messages.OwnedByUser(userId);
            foreach (var message in messages)
            {
                this.unitOfWork.Messages.Remove(message);
            }
        }

        private void UpdateMessagesSentByUser(string userId)
        {
            var messages = this.unitOfWork.Messages.SentByUser(userId);
            foreach(var message in messages)
            {
                message.FromId = null;
            }
        }

        private void UpdateMessagesReceivedByUser(string userId)
        {
            var messages = this.unitOfWork.Messages.ReceivedByUser(userId);
            foreach (var message in messages)
            {
                message.RecipientId = null;
            }
        }
    }
}
