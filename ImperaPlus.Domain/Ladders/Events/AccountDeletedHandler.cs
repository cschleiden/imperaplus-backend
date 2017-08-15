using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Users;

namespace ImperaPlus.Domain.Ladders.Events
{
    public class AccountDeletedHandler : IEventHandler<AccountDeleted>
    {
        private IUnitOfWork unitOfWork;
        private ILadderService ladderService;

        public AccountDeletedHandler(IUnitOfWork unitOfWork, ILadderService ladderService)
        {
            this.unitOfWork = unitOfWork;
            this.ladderService = ladderService;
        }

        public void Handle(AccountDeleted evt)
        {
            var ladders = this.ladderService.GetQueuedLadders(evt.User).ToArray();

            foreach(var ladder in ladders)
            {
                this.ladderService.LeaveQueue(ladder.Id, evt.User);
            }
        }
    }
}
