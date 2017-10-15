using System;

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
        }

        public AllianceJoinRequest(Alliance alliance, User requestedBy)
        {
            this.Alliance = alliance;
            this.AllianceId = alliance.Id;
            this.RequestedByUser = requestedBy;
            this.RequestedByUserId = requestedBy.Id;
        }

        public Guid Id { get; set; }

        public Guid AllianceId { get; set; }
        public virtual Alliance Alliance { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public string RequestedByUserId { get; set; }
        public virtual User RequestedByUser { get; set; }

        public string DeniedByUserID { get; set; }
        public virtual User DeniedByUser { get; set; }

        public AllianceJoinRequestState State { get; set; }
    }
}
