using AutoMapper;
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
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IUserProvider userProvider)
            : base(unitOfWork, mapper, userProvider)
        {
        }

        public DTO.Notifications.NotificationSummary GetSummary()
        {
            var userID = userProvider.GetCurrentUserId();

            return new DTO.Notifications.NotificationSummary
            {
                NumberOfGames = UnitOfWork.Games.CountForUserAtTurn(userID),
                NumberOfMessages = UnitOfWork.Messages.CountUnread(userID)
            };
        }
    }
}
