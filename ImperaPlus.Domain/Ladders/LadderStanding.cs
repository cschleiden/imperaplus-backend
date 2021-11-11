using System;
using ImperaPlus.Domain.Annotations;

namespace ImperaPlus.Domain.Ladders
{
    public class LadderStanding
    {
        [UsedImplicitly]
        protected LadderStanding()
        {
        }

        public LadderStanding(Ladder ladder, User user)
        {
            LadderId = ladder.Id;
            UserId = user.Id;
        }

        public Guid LadderId { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public int GamesPlayed { get; set; }

        public int GamesWon { get; set; }

        public int GamesLost { get; set; }

        public DateTime LastGame { get; set; }

        public double Rating { get; set; }

        public double Vol { get; set; }

        public double Rd { get; set; }
    }
}
