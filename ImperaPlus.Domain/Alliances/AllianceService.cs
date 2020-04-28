using System;
using System.Linq;
using System.Collections.Generic;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Utilities;

namespace ImperaPlus.Domain.Alliances
{
    public interface IAllianceService
    {
        Alliance Get(Guid allianceId);

        IEnumerable<Alliance> GetAll();

        Alliance Create(string name, string description);

        void Delete(Guid allianceId);

        void Leave(Guid allianceId);

        void RemoveMember(Guid allianceId, string userId);

        void ChangeAdmin(Guid allianceId, string userId, bool isAdmin);

        /// <summary>
        /// Get all requests for the calling user
        /// </summary>
        IEnumerable<AllianceJoinRequest> GetJoinRequests();

        IEnumerable<AllianceJoinRequest> GetJoinRequests(Guid allianceId);

        AllianceJoinRequest RequestToJoin(Guid allianceId, string reason);

        AllianceJoinRequest UpdateRequest(Guid allianceId, Guid requestId, AllianceJoinRequestState state);
    }

    public class AllianceService : BaseDomainService, IAllianceService
    {
        private IRandomGen randomGen;

        public AllianceService(IUnitOfWork UnitOfWork, IRandomGen randomGen, IUserProvider userProvider)
            : base(UnitOfWork, userProvider)
        {
            this.randomGen = randomGen;
        }

        public IEnumerable<Alliance> GetAll()
        {
            return this.UnitOfWork.Alliances.GetAll();
        }

        public Alliance Get(Guid allianceId)
        {
            return this.UnitOfWork.Alliances.Get(allianceId);
        }

        public Alliance Create(string name, string description)
        {
            Require.NotNullOrEmpty(name, nameof(name));
            Require.NotNullOrEmpty(description, nameof(description));

            var creator = this.CurrentUser;
            if (creator.AllianceId.HasValue)
            {
                throw new DomainException(ErrorCode.UserAlreadyInAlliance, "User {0} is already in an alliance", creator.Id);
            }

            var allianceWithSameName = this.UnitOfWork.Alliances.FindByName(name);
            if (allianceWithSameName != null)
            {
                throw new DomainException(ErrorCode.AllianceWithNameAlreadyExists, "Alliance with that name already exists");
            }

            Alliance alliance = new Alliance(name, description);
            alliance.AddMember(creator);
            alliance.MakeAdmin(creator);

            this.UnitOfWork.Alliances.Add(alliance);

            return alliance;
        }

        public void Delete(Guid allianceId)
        {
            Require.NotEmpty(allianceId, nameof(allianceId));

            Alliance alliance = this.GetAlliance(allianceId);
            this.CheckAdmin(alliance);

            this.UnitOfWork.Alliances.Remove(alliance);
        }

        public void RemoveMember(Guid allianceId, string userId)
        {
            Require.NotEmpty(allianceId, nameof(allianceId));
            Require.NotNullOrEmpty(userId, nameof(userId));

            var alliance = this.GetAlliance(allianceId);

            if (string.Equals(this.CurrentUser.Id, userId, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Leave(allianceId);
            }
            else
            {
                // Users can remove themselves from an alliance, if the user to be removed is not the calling user, check for admin rights
                this.CheckAdmin(alliance);

                var user = this.GetUser(userId);
                alliance.RemoveMember(user);
            }
        }

        public void ChangeAdmin(Guid allianceId, string userId, bool isAdmin)
        {
            Alliance alliance = this.GetAlliance(allianceId);
            this.CheckAdmin(alliance);

            var user = this.GetUser(userId);

            if (isAdmin)
            {
                alliance.MakeAdmin(user);
            }
            else
            {
                alliance.RemoveAdmin(user);
            }
        }

        public IEnumerable<AllianceJoinRequest> GetJoinRequests(Guid allianceId)
        {
            Require.NotEmpty(allianceId, nameof(allianceId));

            Alliance alliance = this.GetAlliance(allianceId);
            this.CheckAdmin(alliance);

            return alliance.Requests;
        }

        public void Leave(Guid allianceId)
        {
            Require.NotEmpty(allianceId, nameof(allianceId));

            var user = this.CurrentUser;

            Alliance alliance = this.GetAlliance(allianceId);
            if (!alliance.IsMember(user))
            {
                throw new DomainException(
                    ErrorCode.UserNotAMemberOfAlliance,
                    "User {0} is not a member of the alliance {1}", user.Id, allianceId);
            }

            bool isAdmin = alliance.IsAdmin(user);
            if (isAdmin)
            {
                bool isOnlyAdmin = alliance.Administrators.Count() == 1;

                // User was the only admin in the alliance
                if (isOnlyAdmin)
                {
                    bool isOnlyMember = alliance.Members.Count() == 1;
                    if (isOnlyMember)
                    {
                        // User was the only member, we can delete the alliance
                        this.Delete(allianceId);
                    }
                    else
                    {
                        // There are other members, make one admin
                        var newAdmin = alliance.Members.Shuffle(this.randomGen).First(x => x.IsAllianceAdmin == false);
                        alliance.MakeAdmin(newAdmin);
                    }
                }
            }

            user.IsAllianceAdmin = false;
            user.Alliance = null;
            user.AllianceId = null;
        }

        private Alliance GetAlliance(Guid allianceId)
        {
            Require.NotEmpty(allianceId, nameof(allianceId));

            Alliance alliance = this.UnitOfWork.Alliances.Get(allianceId);
            if (alliance == null)
            {
                throw new DomainException(
                    ErrorCode.AllianceNotFound,
                    "Alliance {0} not found", allianceId);
            }

            return alliance;
        }

        private void CheckAdmin(Alliance alliance)
        {
            bool isAdmin = alliance.IsAdmin(this.CurrentUser);
            if (!isAdmin)
            {
                throw new DomainException(
                    ErrorCode.AllianceUserIsNotAdmin,
                    "User {0} is not an admin of alliance {1}", this.CurrentUser.Id, alliance.Id);
            }
        }

        public AllianceJoinRequest RequestToJoin(Guid allianceId, string reason)
        {
            var alliance = this.GetAlliance(allianceId);
            return alliance.RequestToJoin(this.CurrentUser, reason);
        }

        public AllianceJoinRequest UpdateRequest(Guid allianceId, Guid requestId, AllianceJoinRequestState state)
        {
            var alliance = this.GetAlliance(allianceId);
            this.CheckAdmin(alliance);

            if (state == AllianceJoinRequestState.Approved)
            {
                // Approve request
                return alliance.ApproveRequest(this.CurrentUser, requestId);
            }
            else if (state == AllianceJoinRequestState.Denied)
            {
                return alliance.DenyRequest(this.CurrentUser, requestId);
            }
            else
            {
                throw new DomainException(ErrorCode.InvalidAllianceJoinRequestState, "Invalid state for join request");
            }
        }

        public IEnumerable<AllianceJoinRequest> GetJoinRequests()
        {
            return this.UnitOfWork.Alliances.GetRequestsForUser(this.CurrentUser.Id);
        }
    }
}
