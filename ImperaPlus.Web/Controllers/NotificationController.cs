using ImperaPlus.Application.Notifications;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.DTO;
using ImperaPlus.DTO.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace ImperaPlus.Backend.Controllers
{
    [Authorize]
    [Route("notifications")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(void), 200)]
    public class NotificationController : BaseController
    {
        private INotificationService notificationService;

        public NotificationController(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
            : base(unitOfWork, mapper)
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
            return Ok(notificationService.GetSummary());
        }
    }
}
