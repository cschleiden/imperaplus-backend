using ImperaPlus.Application.Notifications;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.DTO;
using ImperaPlus.DTO.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Controllers
{
    [Authorize]
    [Route("api/notifications")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(void), 200)]
    public class NotificationController : BaseController
    {
        private INotificationService notificationService;

        public NotificationController(IUnitOfWork unitOfWork, INotificationService notificationService)
            : base(unitOfWork)
        {
            this.notificationService = notificationService;
        }

        /// <summary>
        /// Get notification summary for current user
        /// </summary>
        [HttpGet("summary")]
        [ProducesResponseType(typeof(NotificationSummary), 200)]
        public IActionResult GetSummary()
        {
            return this.Ok(this.notificationService.GetSummary());
        }
    }
}
