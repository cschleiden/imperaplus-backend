using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.Application.Notifications
{
    public interface INotificationService
    {
        /// <summary>
        /// Get notification summary for current user
        /// </summary>        
        DTO.Notifications.NotificationSummary GetSummary();
    }

    public class NotificationService : BaseService, INotificationService
    {
        public NotificationService(IUnitOfWork unitOfWork, IUserProvider userProvider)
            : base(unitOfWork, userProvider)
        {
        }

        public DTO.Notifications.NotificationSummary GetSummary()
        {
            var userID = this.userProvider.GetCurrentUserId();

            return new DTO.Notifications.NotificationSummary
            {
                NumberOfGames = this.UnitOfWork.Games.CountForUserAtTurn(userID),
                NumberOfMessages = this.UnitOfWork.Messages.CountUnread(userID)
            };
        }
    }
}
