using System.Linq;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Users;

namespace ImperaPlus.Domain.Alliances.EventHandler
{
    public class AccountDeletedHandler : IEventHandler<AccountDeleted>
    {
        private IUnitOfWork unitOfWork;
        private IAllianceService allianceService;

        public AccountDeletedHandler(IUnitOfWork unitOfWork, IAllianceService allianceService)
        {
            this.unitOfWork = unitOfWork;
            this.allianceService = allianceService;
        }

        public void Handle(AccountDeleted evt)
        {
            var user = evt.User;

            // Remove from alliance, if a member
            if (user.AllianceId.HasValue)
            {
                this.allianceService.Leave(user.AllianceId.Value, user);
            }

            // Remove alliance join requests
            this.unitOfWork.GetGenericRepository<AllianceJoinRequest>()
                .Query()
                .Where(x => x.ApprovedByUserId == user.Id || x.DeniedByUserId == user.Id)
                .ToList()
                .ForEach(x => this.unitOfWork.GetGenericRepository<AllianceJoinRequest>().Remove(x));
        }
    }
}
