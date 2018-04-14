using System;

namespace ImperaPlus.DTO.Alliances
{
    public class AllianceJoinRequest
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Date and time when the request was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Id of alliance to join
        /// </summary>
        public Guid AllianceId { get; set; }
    
        /// <summary>
        /// Date and time when the request was last modified
        /// </summary>
        public DateTime LastModifiedAt { get; set; }

        /// <summary>
        /// Id of user wishing to join the alliance
        /// </summary>
        public string RequestedByUserId { get; private set; }

        /// <summary>
        /// State of the request
        /// </summary>
        public AllianceJoinRequestState State { get; set; }

        /// <summary>
        /// Reason why user wants to join alliance
        /// </summary>
        public string Reason { get; set; }
    }
}
