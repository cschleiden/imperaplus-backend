using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ImperaPlus.Application.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Controllers
{
    [Authorize]
    [Route("api/messages")]
    public class MessageController : BaseController
    {
        private IMessageService messageService;

        public MessageController(IMessageService messageService)
        {
            this.messageService = messageService;   
        }

        [HttpGet("folder/{folder}")]
        [Produces(typeof(IEnumerable<DTO.Messages.Message>))]
        public IActionResult Get(DTO.Messages.MessageFolder folder = DTO.Messages.MessageFolder.Inbox)
        {
            return this.Ok(this.messageService.Get(folder));
        }

        [HttpGet("{messageId}")]
        [Produces(typeof(DTO.Messages.Message))]
        public IActionResult Get(Guid messageId)
        {
            return this.Ok(this.messageService.Get(messageId));
        }

        [HttpGet("folders")]
        [Produces(typeof(DTO.Messages.FolderInformation))]
        public IActionResult GetFolderInformation()
        {
            return this.Ok(this.messageService.GetFolderInformation());
        }
        
        [HttpPost("")]
        public IActionResult PostSend(DTO.Messages.SendMessage message)
        {
            var subject = Regex.Replace(message.Subject, @"<[^>]*>", string.Empty); 
            var text = Regex.Replace(message.Text, @"<[^>]*>", string.Empty);

            var id = this.messageService.SendMessage(message.To.Id, subject, text);

            return this.Ok(id);
        }
        
        [HttpPatch("{messageId:guid}")]
        public IActionResult PatchMarkRead(Guid messageId)
        {
            this.messageService.MarkRead(messageId);

            return this.Ok();
        }
        
        [HttpDelete("{messageId:guid}")]
        public IActionResult Delete(Guid messageId)
        {
            this.messageService.Delete(messageId);

            return this.Ok();
        }
    }
}
