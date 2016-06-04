using System.Web.Http;
using ImperaPlus.Application.Notifications;

namespace ImperaPlus.Backend.Controllers
{
    [Authorize]
    [RoutePrefix("api/notifications")]
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
        [HttpGet]
        [Route("summary")]
        public IHttpActionResult GetSummary()
        {
            return this.Ok(this.notificationService.GetSummary());
        }
    }
}
