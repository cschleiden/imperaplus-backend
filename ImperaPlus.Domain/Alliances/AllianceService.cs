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
        Alliance Create(User creator, string name, string description);

        void Delete(Guid allianceId, User admin);

        void RequestToJoin(Guid allianceId, User user, string reason);

        void ApproveRequestToJoin(Guid allianceId, User approver, User user);

        void DenyRequestToJoin(Guid allianceId, User denier, User user);

        void Leave(Guid allianceId, User user);

        void MakeAdmin(Guid allianceId, User currentAdmin, User user);

        IEnumerable<Alliance> GetAll();

        Alliance Get(Guid allianceId);
    }

    public class AllianceService : IAllianceService
    {
        private IUnitOfWork unitOfWork;
        private IRandomGen randomGen;

        public AllianceService(IUnitOfWork unitOfWork, IRandomGen randomGen)
        {
            this.unitOfWork = unitOfWork;
            this.randomGen = randomGen;
        }

        public IEnumerable<Alliance> GetAll()
        {
            return this.unitOfWork.Alliances.GetAll();
        }

        public Alliance Get(Guid allianceId)
        {
            return this.unitOfWork.Alliances.GetWithMembers(allianceId);
        }

        public Alliance Create(User creator, string name, string description)
        {
            Require.NotNull(creator, nameof(creator));
            Require.NotNullOrEmpty(name, nameof(name));
            Require.NotNullOrEmpty(description, nameof(description));

            if (creator.AllianceId.HasValue)
            {
                throw new DomainException(ErrorCode.UserAlreadyInAlliance, "User {0} is already in an alliance", creator.Id);
            }

            var allianceWithSameName = this.unitOfWork.Alliances.FindByName(name);
            if (allianceWithSameName != null)           
            {
                throw new DomainException(ErrorCode.AllianceWithNameAlreadyExists, "Alliance with that name already exists");
            }

            Alliance alliance = new Alliance(name, description);
            alliance.AddMember(creator);

            return alliance;
        }

        public void Delete(Guid allianceId, User admin)
        {
            Require.NotEmpty(allianceId, nameof(allianceId));

            Alliance alliance = this.GetAlliance(allianceId);

            this.CheckAdmin(alliance, admin);

            this.unitOfWork.Alliances.Remove(alliance);
        }

        public void MakeAdmin(Guid allianceId, User currentAdmin, User user)
        {
            Require.NotEmpty(allianceId, nameof(allianceId));
            Require.NotNull(currentAdmin, nameof(currentAdmin));
            Require.NotNull(user, nameof(user));

            Alliance alliance = this.GetAlliance(allianceId);

            this.CheckAdmin(alliance, currentAdmin);

            alliance.MakeAdmin(user);
        }

        public void RequestToJoin(Guid allianceId, User user, string reason)
        {
            Require.NotEmpty(allianceId, nameof(allianceId));
            Require.NotNull(user, nameof(user));
            
            Alliance alliance = this.GetAlliance(allianceId);            
            
            // Create new request
            alliance.RequestToJoin(user, reason);
        }

        public void ApproveRequestToJoin(Guid allianceId, User approver, User user)
        {
            Require.NotEmpty(allianceId, nameof(allianceId));
            Require.NotNull(approver, nameof(approver));
            Require.NotNull(user, nameof(user));

            Alliance alliance = this.GetAlliance(allianceId);

            this.CheckAdmin(alliance, approver);

            alliance.ApproveRequest(approver, user);
        }

        public void DenyRequestToJoin(Guid allianceId, User denier, User user)
        {
            Require.NotEmpty(allianceId, nameof(allianceId));
            Require.NotNull(denier, nameof(denier));
            Require.NotNull(user, nameof(user));

            Alliance alliance = this.GetAlliance(allianceId);

            this.CheckAdmin(alliance, denier);

            alliance.DenyRequest(denier, user);
        }

        public void Leave(Guid allianceId, User user)
        {
            Require.NotNull(user, nameof(user));
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
                        this.Delete(allianceId, user);
                    }
                    else
                    {
                        // There are other members, make one admin
                        var newAdmin = alliance.Members.Shuffle(this.randomGen).First(x => x.IsAllianceAdmin == false);
                        alliance.MakeAdmin(newAdmin);
                    }
                }
            }

            user.Alliance = null;
            user.AllianceId = null;
        }

        private Alliance GetAlliance(Guid allianceId)
        {
            Alliance alliance = this.unitOfWork.Alliances.GetWithMembers(allianceId);
            if (alliance == null)
            {
                throw new DomainException(
                    ErrorCode.AllianceNotFound, 
                    "Alliance {0} not found", allianceId);
            }

            return alliance;
        }
        
        private void CheckAdmin(Alliance alliance, User admin)
        {
            bool isAdmin = alliance.IsAdmin(admin);
            if (!isAdmin)
            {
                throw new DomainException(
                    ErrorCode.AllianceUserIsNotAdmin, 
                    "User {0} is not an admin of alliance {1}", admin.Id, alliance.Id);
            }
        }
    }
}
