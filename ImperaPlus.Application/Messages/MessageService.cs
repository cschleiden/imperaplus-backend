using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.Application.Messages
{
    public interface IMessageService
    {
        DTO.Messages.Message Get(Guid id);

        IEnumerable<DTO.Messages.Message> Get(DTO.Messages.MessageFolder folder = DTO.Messages.MessageFolder.Inbox);

        IEnumerable<DTO.Messages.FolderInformation> GetFolderInformation();

        Guid SendMessage(string toId, string subject, string text);

        void MarkRead(Guid messageId);

        void Delete(Guid messageId);
    }

    public class MessageService : BaseService, IMessageService
    {
        private IUserNotificationService userNotificationService;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper, IUserProvider userProvider,
            IUserNotificationService userNotificationService)
            : base(unitOfWork, mapper, userProvider)
        {
            this.userNotificationService = userNotificationService;
        }

        public DTO.Messages.Message Get(Guid id)
        {
            var currentUserId = userProvider.GetCurrentUserId();

            var message = UnitOfWork.Messages
                .Query()
                .Include(x => x.Owner)
                .Include(x => x.Recipient)
                .Include(x => x.From)
                .Where(m => m.OwnerId == currentUserId && m.Id == id)
                .FirstOrDefault();

            if (message == null)
            {
                throw new Exceptions.ApplicationException("Cannot find message", ErrorCode.CannotFindMessage);
            }

            return Mapper.Map<DTO.Messages.Message>(message);
        }

        public IEnumerable<DTO.Messages.Message> Get(
            DTO.Messages.MessageFolder folder = DTO.Messages.MessageFolder.Inbox)
        {
            var mappedFolder = Mapper.Map<Domain.Messages.MessageFolder>(folder);
            var currentUserId = userProvider.GetCurrentUserId();

            return Mapper.Map<IEnumerable<DTO.Messages.Message>>(UnitOfWork.Messages
                .Query()
                .Include(x => x.Owner)
                .Include(x => x.Recipient)
                .Include(x => x.From)
                .Where(m => m.Folder == mappedFolder && m.OwnerId == currentUserId));
        }

        public IEnumerable<DTO.Messages.FolderInformation> GetFolderInformation()
        {
            var currentUserId = userProvider.GetCurrentUserId();

            var messagesByFolder = UnitOfWork.Messages
                .Query()
                .Where(m => m.OwnerId == currentUserId)
                .GroupBy(m => m.Folder)
                .Select(gr => new { Folder = gr.Key, Count = gr.Count() })
                .ToDictionary(x => x.Folder);

            var unreadMessagesByFolder = UnitOfWork.Messages
                .Query()
                .Where(m => m.OwnerId == currentUserId && !m.IsRead)
                .GroupBy(m => m.Folder)
                .Select(gr => new { Folder = gr.Key, Count = gr.Count() })
                .ToDictionary(x => x.Folder, x => x.Count);

            return new[] { Domain.Messages.MessageFolder.Inbox, Domain.Messages.MessageFolder.Sent }.Select(folder =>
            {
                var mappedFolder = Mapper.Map<DTO.Messages.MessageFolder>(folder);

                if (messagesByFolder.ContainsKey(folder))
                {
                    return new DTO.Messages.FolderInformation
                    {
                        Folder = mappedFolder,
                        Count = messagesByFolder[folder].Count,
                        UnreadCount = unreadMessagesByFolder.GetValueOrDefault(folder)
                    };
                }
                else
                {
                    return new DTO.Messages.FolderInformation { Folder = mappedFolder, Count = 0, UnreadCount = 0 };
                }
            });
        }

        public void MarkRead(Guid messageId)
        {
            Require.NotEmpty(messageId, nameof(messageId));

            var message = UnitOfWork.Messages.FindById(messageId);

            if (message.OwnerId != userProvider.GetCurrentUserId())
            {
                throw new Exceptions.ApplicationException("Can only mark own messages as read",
                    ErrorCode.UserIsNotAllowedToPerformAction);
            }

            message.IsRead = true;

            UnitOfWork.Commit();
        }

        public void Delete(Guid messageId)
        {
            Require.NotEmpty(messageId, nameof(messageId));

            var message = UnitOfWork.Messages.FindById(messageId);
            if (message.OwnerId != userProvider.GetCurrentUserId())
            {
                throw new Exceptions.ApplicationException("Can only mark own messages as read",
                    ErrorCode.UserIsNotAllowedToPerformAction);
            }

            UnitOfWork.Messages.Remove(message);
            UnitOfWork.Commit();
        }

        public Guid SendMessage(string toId, string subject, string text)
        {
            Require.NotNullOrEmpty(toId, nameof(toId));
            Require.NotNullOrEmpty(subject, nameof(subject));
            Require.NotNullOrEmpty(text, nameof(text));

            var toUser = UnitOfWork.Users.FindById(toId);
            if (toUser == null)
            {
                throw new Exceptions.ApplicationException("Cannot find user", ErrorCode.UserDoesNotExist);
            }

            var messageFrom = new Domain.Messages.Message(CurrentUser, CurrentUser, toUser, subject, text,
                Domain.Messages.MessageFolder.Sent);
            messageFrom.IsRead = true;
            var messageTo = new Domain.Messages.Message(toUser, CurrentUser, toUser, subject, text,
                Domain.Messages.MessageFolder.Inbox);

            UnitOfWork.Messages.Add(messageFrom);
            UnitOfWork.Messages.Add(messageTo);

            // Notify recipient
            userNotificationService.SendNotification(toUser.Id,
                new DTO.Notifications.NewMessageNotification
                {
                    FromUserName = CurrentUser.UserName, Subject = subject
                });

            UnitOfWork.Commit();

            return messageTo.Id;
        }
    }
}
