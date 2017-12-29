using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Users;

namespace ImperaPlus.Domain.Chat.EventHandler
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
            var messagesFromUser = this.unitOfWork.ChatMessages.FindForUser(evt.User);
            foreach(var message in messagesFromUser)
            {
                message.CreatedBy = null;
                message.CreatedById = null;

                // TODO: Update connected clients
                // this.chatService.DeleteMessage(message.Id);
            }
        }
    }
}
