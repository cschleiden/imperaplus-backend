using System;
using System.Text.RegularExpressions;
using System.Web.Http;
using ImperaPlus.Application.Messages;
using System.Web.Http.Description;
using System.Collections.Generic;

namespace ImperaPlus.Backend.Controllers
{
    [Authorize]
    [RoutePrefix("api/messages")]
    public class MessageController : BaseController
    {
        private IMessageService messageService;

        public MessageController(IMessageService messageService)
        {
            this.messageService = messageService;   
        }

        [Route("folder/{folder}")]
        [ResponseType(typeof(IEnumerable<DTO.Messages.Message>))]
        public IHttpActionResult Get(DTO.Messages.MessageFolder folder = DTO.Messages.MessageFolder.Inbox)
        {
            return this.Ok(this.messageService.Get(folder));
        }

        [Route("{messageId}")]
        [ResponseType(typeof(DTO.Messages.Message))]
        public IHttpActionResult Get(Guid messageId)
        {
            return this.Ok(this.messageService.Get(messageId));
        }

        [Route("folders")]
        [ResponseType(typeof(DTO.Messages.FolderInformation))]
        public IHttpActionResult GetFolderInformation()
        {
            return this.Ok(this.messageService.GetFolderInformation());
        }
        
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostSend(DTO.Messages.SendMessage message)
        {
            var subject = Regex.Replace(message.Subject, @"<[^>]*>", string.Empty); 
            var text = Regex.Replace(message.Text, @"<[^>]*>", string.Empty);

            var id = this.messageService.SendMessage(message.To.Id, subject, text);

            return this.Ok(id);
        }

        [HttpPatch]
        [Route("{messageId:guid}")]
        public IHttpActionResult PatchMarkRead(Guid messageId)
        {
            this.messageService.MarkRead(messageId);

            return this.Ok();
        }

        [HttpDelete]
        [Route("{messageId:guid}")]
        public IHttpActionResult Delete(Guid messageId)
        {
            this.messageService.Delete(messageId);

            return this.Ok();
        }
    }
}
