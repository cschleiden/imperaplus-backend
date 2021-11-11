using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ImperaPlus.Application.Messages;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace ImperaPlus.Backend.Controllers
{
    [Authorize]
    [Route("messages")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(void), 200)]
    public class MessageController : BaseController
    {
        private IMessageService messageService;

        public MessageController(IUnitOfWork unitOfWork, IMapper mapper, IMessageService messageService)
            : base(unitOfWork, mapper)
        {
            this.messageService = messageService;
        }

        [HttpGet("folder/{messageFolder}")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Messages.Message>), 200)]
        public IActionResult Get(DTO.Messages.MessageFolder messageFolder = DTO.Messages.MessageFolder.Inbox)
        {
            return Ok(messageService.Get(messageFolder));
        }

        [HttpGet("{messageId}")]
        [ProducesResponseType(typeof(DTO.Messages.Message), 200)]
        public IActionResult Get(Guid messageId)
        {
            return Ok(messageService.Get(messageId));
        }

        [HttpGet("folders")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Messages.FolderInformation>), 200)]
        public IActionResult GetFolderInformation()
        {
            return Ok(messageService.GetFolderInformation());
        }

        [HttpPost("")]
        public IActionResult PostSend([FromBody] DTO.Messages.SendMessage message)
        {
            var subject = Regex.Replace(message.Subject, @"<[^>]*>", string.Empty);
            var text = Regex.Replace(message.Text, @"<[^>]*>", string.Empty);

            var id = messageService.SendMessage(message.To.Id, subject, text);

            return Ok(id);
        }

        [HttpPatch("{messageId:guid}")]
        public IActionResult PatchMarkRead(Guid messageId)
        {
            messageService.MarkRead(messageId);

            return Ok();
        }

        [HttpDelete("{messageId:guid}")]
        public IActionResult Delete(Guid messageId)
        {
            messageService.Delete(messageId);

            return Ok();
        }
    }
}
