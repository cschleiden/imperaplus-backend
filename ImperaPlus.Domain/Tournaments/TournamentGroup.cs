using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Utilities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImperaPlus.Domain.Tournaments
{
    public class TournamentGroup
    {
        protected TournamentGroup()
        {
            this.Teams = new HashSet<TournamentTeam>();
            this.Pairings = new HashSet<TournamentPairing>();
        }

        public TournamentGroup(Tournament tournament, int number)
            : this()
        {
            Require.NotNull(tournament, nameof(tournament));

            this.Id = Guid.NewGuid();

            this.TournamentId = tournament.Id;
            this.Tournament = tournament;

            if (number <= 0)
            {
                throw new ArgumentException(nameof(number));
            }

            this.Number = number;
        }

        public Guid Id { get; protected set; }

        public virtual ICollection<TournamentTeam> Teams { get; protected set; }
        public virtual ICollection<TournamentPairing> Pairings { get; protected set; }

        public Guid TournamentId { get; protected set; }
        public virtual Tournament Tournament { get; protected set; }

        public int Number { get; protected set; }

        [NotMapped]
        public IEnumerable<TournamentTeam> Winners
        {
            get
            {
                return this.Teams
                    .OrderBy(t => t.GroupOrder)
                    .Take(2);
            }
        }

        [NotMapped]
        public IEnumerable<TournamentTeam> Losers
        {
            get
            {
                return this.Teams
                    .OrderBy(t => t.GroupOrder)
                    .Skip(2);
            }
        }
    }
}
