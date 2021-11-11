using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImperaPlus.Domain.Tournaments
{
    public class TournamentParticipant
    {
        protected TournamentParticipant()
        {
            Id = Guid.NewGuid();
        }

        public TournamentParticipant(TournamentTeam team, User user)
            : this()
        {
            TeamId = team.Id;
            Team = team;

            UserId = user.Id;
            User = user;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public string UserId { get; protected set; }
        public virtual User User { get; protected set; }

        public Guid TeamId { get; protected set; }
        public virtual TournamentTeam Team { get; protected set; }
    }
}
