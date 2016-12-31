using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AspNet.Security.OAuth.Validation;
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
        [ProducesResponseType(typeof(IEnumerable<DTO.Messages.Message>), 200)]
        public IActionResult Get(DTO.Messages.MessageFolder messageFolder = DTO.Messages.MessageFolder.Inbox)
        {
            return this.Ok(this.messageService.Get(messageFolder));
        }

        [HttpGet("{messageId}")]
        [ProducesResponseType(typeof(DTO.Messages.Message), 200)]
        public IActionResult Get(Guid messageId)
        {
            return this.Ok(this.messageService.Get(messageId));
        }

        [HttpGet("folders")]
        [ProducesResponseType(typeof(DTO.Messages.FolderInformation), 200)]
        public IActionResult GetFolderInformation()
        {
            return this.Ok(this.messageService.GetFolderInformation());
        }
        
        [HttpPost("")]
        public IActionResult PostSend([FromBody] DTO.Messages.SendMessage message)
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
