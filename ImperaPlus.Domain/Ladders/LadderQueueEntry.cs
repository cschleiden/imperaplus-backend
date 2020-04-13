using System;
using ImperaPlus.Domain.Annotations;

namespace ImperaPlus.Domain.Ladders
{
    public class LadderQueueEntry : IChangeTrackedEntity
    {
        private Ladder ladder;

        [UsedImplicitly]
        protected LadderQueueEntry()
        {
        }

        public LadderQueueEntry(Ladder ladder, User user)
        {
            this.ladder = ladder;

            this.User = user;
            this.UserId = user.Id;
        }

        public Guid LadderId { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }
}
