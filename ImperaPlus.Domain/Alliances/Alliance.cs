using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ImperaPlus.Domain.Chat;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Utilities;

namespace ImperaPlus.Domain.Alliances
{
    public class Alliance
    {
        protected Alliance()
        {
            this.Id = Guid.NewGuid();
            this.Members = new HashSet<User>();
            this.Requests = new HashSet<AllianceJoinRequest>();
        }

        public Alliance(string name, string description)
            : this()
        {
            this.Name = name;
            this.Description = description;
        }

        public Guid Id { get; protected set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [NotMapped]
        public IEnumerable<User> Administrators
        {
            get
            {
                return this.Members.Where(x => x.IsAllianceAdmin);
            }
        }

        public virtual ICollection<User> Members { get; private set; }

        public virtual ICollection<AllianceJoinRequest> Requests { get; private set; }

        public Guid ChannelId { get; set; }
        public virtual Channel Channel { get; set; }

        public void AddMember(User user)
        {
            bool isMemberAlready = this.IsMember(user);
            if (isMemberAlready)
            {
                throw new DomainException(
                    ErrorCode.UserAlreadyInAlliance, 
                    "User {0} is already a member of alliance {1}", user.Id, this.Id);
            }

            this.Members.Add(user);
        }

        public bool IsMember(User user)
        {
            return this.Members.Any(m => m.Id == user.Id);
        }

        internal void MakeAdmin(User user)
        {
            Require.NotNull(user, nameof(user));

            bool isMember = this.IsMember(user);
            if (!isMember)
            {
                throw new DomainException(
                    ErrorCode.UserNotAMemberOfAlliance, 
                    "User {0} is not a member of alliance {1}", user.Id, this.Id);
            }

            bool isAdminAlready = this.IsAdmin(user);
            if (isAdminAlready)
            {
                throw new DomainException(
                    ErrorCode.UserAlreadyAllianceAdmin,
                    "User {0} is already an admin of alliance {1}", user.Id, this.Id);
            }

            user.IsAllianceAdmin = true;
        }

        /// <summary>
        /// Returns a value indicating whether the given user is an alliance admin
        /// </summary>
        public bool IsAdmin(User currentAdmin)
        {
            Require.NotNull(currentAdmin, nameof(currentAdmin));

            return this.Administrators.Any(x => x.Id == currentAdmin.Id);
        }

        public void RemoveMember(User user)
        {
            Require.NotNull(user, nameof(user));

            bool isMember = this.IsMember(user);
            if (!isMember)
            {
                throw new DomainException(
                    ErrorCode.UserNotAMemberOfAlliance,
                    "User {0} is not a member of alliance {1}", user.Id, this.Id);
            }

            var member = this.Members.FirstOrDefault(x => x.Id == user.Id);
            this.Members.Remove(member);
        }

        public AllianceJoinRequest RequestToJoin(User user, string reason)
        {
            bool isMemberAlready = this.IsMember(user);
            if (isMemberAlready)
            {
                throw new DomainException(
                    ErrorCode.UserAlreadyInAlliance,
                    "User {0} is already a member of alliance {1}", user.Id, this.Id);
            }

            var activeRequest = this.FindActiveRequestForUser(user);
            if (activeRequest != null)
            {
                throw new DomainException(
                    ErrorCode.ActiveRequestToJoinAllianceExists,
                    "There is already an active request to join the alliance for thi user");
            }

            var request = new AllianceJoinRequest(this, user, reason);
            this.Requests.Add(request);

            return request;
        }

        /// <summary>
        /// Approves the most request active request for a user if it exists
        /// </summary>
        public void ApproveRequest(User approver, User user)
        {
            var activeRequest = this.GetActiveRequestForUser(user);

            this.AddMember(user);
            activeRequest.Approve(approver);
        }

        public void DenyRequest(User denier, User user)
        {
            var activeRequest = this.GetActiveRequestForUser(user);
            
            activeRequest.Deny(denier);
        }

        private AllianceJoinRequest GetActiveRequestForUser(User user)
        {
            var request = this.FindActiveRequestForUser(user);
            if (request == null)
            {
                throw new DomainException(
                    ErrorCode.NoActiveRequestToJoinAlliance,
                    "There is no active request for user {0} to join alliance {1}", user.Id, this.Id);
            }

            return request;
        }

        private AllianceJoinRequest FindActiveRequestForUser(User user)
        {
            return this.Requests.FirstOrDefault(
                x => x.State == AllianceJoinRequestState.Active && x.RequestedByUserId == user.Id);
        }
    }
}