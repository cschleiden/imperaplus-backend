using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImperaPlus.Domain.Alliances
{
    public enum AllianceJoinRequestState
    {
        Active,

        Approved,

        Denied
    }

    public class AllianceJoinRequest : IIdentifiableEntity<Guid>, IChangeTrackedEntity
    {
        protected AllianceJoinRequest()
        {
            Id = Guid.NewGuid();
        }

        public AllianceJoinRequest(Alliance alliance, User requestedBy, string reason)
            : this()
        {
            Alliance = alliance;
            AllianceId = alliance.Id;
            RequestedByUser = requestedBy;
            RequestedByUserId = requestedBy.Id;

            Reason = reason;

            State = AllianceJoinRequestState.Active;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; private set; }

        public Guid AllianceId { get; private set; }
        public virtual Alliance Alliance { get; private set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public string RequestedByUserId { get; private set; }
        public virtual User RequestedByUser { get; private set; }

        public string Reason { get; private set; }

        public string ApprovedByUserId { get; private set; }
        public virtual User ApprovedByUser { get; private set; }

        public string DeniedByUserId { get; private set; }
        public virtual User DeniedByUser { get; private set; }

        public AllianceJoinRequestState State { get; set; }

        internal void Approve(User approver)
        {
            State = AllianceJoinRequestState.Approved;
            ApprovedByUser = approver;
        }

        internal void Deny(User denier)
        {
            State = AllianceJoinRequestState.Denied;
            DeniedByUser = denier;
        }
    }
}
