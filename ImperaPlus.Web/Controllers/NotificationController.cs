using AspNet.Security.OAuth.Validation;
using ImperaPlus.Application.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Controllers
{
    [Authorize]
    [Route("api/notifications")]
    public class NotificationController : BaseController
    {
        private INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        /// <summary>
        /// Get notification summary for current user
        /// </summary>
        [HttpGet("summary")]
        public IActionResult GetSummary()
        {
            return this.Ok(this.notificationService.GetSummary());
        }
    }
}
