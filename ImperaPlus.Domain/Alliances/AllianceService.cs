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

        void Leave(Guid allianceId, User user = null);

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
            return UnitOfWork.Alliances.GetAll();
        }

        public Alliance Get(Guid allianceId)
        {
            return UnitOfWork.Alliances.Get(allianceId);
        }

        public Alliance Create(string name, string description)
        {
            Require.NotNullOrEmpty(name, nameof(name));
            Require.NotNullOrEmpty(description, nameof(description));

            var creator = CurrentUser;
            if (creator.AllianceId.HasValue)
            {
                throw new DomainException(ErrorCode.UserAlreadyInAlliance, "User {0} is already in an alliance",
                    creator.Id);
            }

            var allianceWithSameName = UnitOfWork.Alliances.FindByName(name);
            if (allianceWithSameName != null)
            {
                throw new DomainException(ErrorCode.AllianceWithNameAlreadyExists,
                    "Alliance with that name already exists");
            }

            var alliance = new Alliance(name, description);
            alliance.AddMember(creator);
            alliance.MakeAdmin(creator);

            UnitOfWork.Alliances.Add(alliance);

            return alliance;
        }

        public void Delete(Guid allianceId)
        {
            Require.NotEmpty(allianceId, nameof(allianceId));

            var alliance = GetAlliance(allianceId);
            CheckAdmin(alliance);

            UnitOfWork.Alliances.Remove(alliance);
        }

        public void RemoveMember(Guid allianceId, string userId)
        {
            Require.NotEmpty(allianceId, nameof(allianceId));
            Require.NotNullOrEmpty(userId, nameof(userId));

            var alliance = GetAlliance(allianceId);

            if (string.Equals(CurrentUser.Id, userId, StringComparison.InvariantCultureIgnoreCase))
            {
                Leave(allianceId);
            }
            else
            {
                // Users can remove themselves from an alliance, if the user to be removed is not the calling user, check for admin rights
                CheckAdmin(alliance);

                var user = GetUser(userId);
                alliance.RemoveMember(user);
            }
        }

        public void ChangeAdmin(Guid allianceId, string userId, bool isAdmin)
        {
            var alliance = GetAlliance(allianceId);
            CheckAdmin(alliance);

            var user = GetUser(userId);

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

            var alliance = GetAlliance(allianceId);
            CheckAdmin(alliance);

            return alliance.Requests;
        }

        public void Leave(Guid allianceId, User user = null)
        {
            Require.NotEmpty(allianceId, nameof(allianceId));

            if (user == null)
            {
                user = CurrentUser;
            }

            var alliance = GetAlliance(allianceId);
            if (!alliance.IsMember(user))
            {
                throw new DomainException(
                    ErrorCode.UserNotAMemberOfAlliance,
                    "User {0} is not a member of the alliance {1}", user.Id, allianceId);
            }

            var isAdmin = alliance.IsAdmin(user);
            if (isAdmin)
            {
                var isOnlyAdmin = alliance.Administrators.Count() == 1;

                // User was the only admin in the alliance
                if (isOnlyAdmin)
                {
                    var isOnlyMember = alliance.Members.Count() == 1;
                    if (isOnlyMember)
                    {
                        // User was the only member, we can delete the alliance
                        Delete(allianceId);
                    }
                    else
                    {
                        // There are other members, make one admin
                        var newAdmin = alliance.Members.Shuffle(randomGen).First(x => x.IsAllianceAdmin == false);
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

            var alliance = UnitOfWork.Alliances.Get(allianceId);
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
            var isAdmin = alliance.IsAdmin(CurrentUser);
            if (!isAdmin)
            {
                throw new DomainException(
                    ErrorCode.AllianceUserIsNotAdmin,
                    "User {0} is not an admin of alliance {1}", CurrentUser.Id, alliance.Id);
            }
        }

        public AllianceJoinRequest RequestToJoin(Guid allianceId, string reason)
        {
            var alliance = GetAlliance(allianceId);
            return alliance.RequestToJoin(CurrentUser, reason);
        }

        public AllianceJoinRequest UpdateRequest(Guid allianceId, Guid requestId, AllianceJoinRequestState state)
        {
            var alliance = GetAlliance(allianceId);
            CheckAdmin(alliance);

            if (state == AllianceJoinRequestState.Approved)
            {
                // Approve request
                return alliance.ApproveRequest(CurrentUser, requestId);
            }
            else if (state == AllianceJoinRequestState.Denied)
            {
                return alliance.DenyRequest(CurrentUser, requestId);
            }
            else
            {
                throw new DomainException(ErrorCode.InvalidAllianceJoinRequestState, "Invalid state for join request");
            }
        }

        public IEnumerable<AllianceJoinRequest> GetJoinRequests()
        {
            return UnitOfWork.Alliances.GetRequestsForUser(CurrentUser.Id);
        }
    }
}
