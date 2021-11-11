using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ImperaPlus.Domain.Exceptions;

namespace ImperaPlus.Domain.Tournaments
{
    public class TournamentTeam : IOwnedEntity
    {
        protected TournamentTeam()
        {
            Id = Guid.NewGuid();

            Participants = new HashSet<TournamentParticipant>();
        }

        public TournamentTeam(Tournament tournament)
            : this()
        {
            Tournament = tournament;
            TournamentId = tournament.Id;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public Guid TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; }

        public virtual ICollection<TournamentParticipant> Participants { get; private set; }

        public string Name { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// Order of team within its group (if group enabled)
        /// </summary>
        public int GroupOrder { get; set; }

        public Guid? GroupId { get; set; }
        public virtual TournamentGroup Group { get; set; }

        public TournamentTeamState State { get; set; }

        public string CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public void AddUser(User user)
        {
            if (Tournament.Options.NumberOfPlayersPerTeam <= Participants.Count())
            {
                throw new DomainException(ErrorCode.TournamentTooManyPlayersInTeam,
                    "Cannot join team, too many players already");
            }

            Participants.Add(new TournamentParticipant(this, user));

            if (Participants.Count() == Tournament.Options.NumberOfPlayersPerTeam)
            {
                State = TournamentTeamState.Active;
            }
        }

        public bool PasswordMatch(string password)
        {
            return string.IsNullOrEmpty(Password) || Password == password;
        }
    }
}
