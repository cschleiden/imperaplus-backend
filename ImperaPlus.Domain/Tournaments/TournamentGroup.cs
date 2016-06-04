using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Utilities;

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

        public IEnumerable<TournamentTeam> Winners
        {
            get
            {
                return this.Pairings
                    .Where(x => x.CanWinnerBeDetermined)
                    .Select(x => x.Winner)
                    .GroupBy(x => x)
                    .OrderByDescending(g => g.Count())
                    .Select(x => x.Key);
            }
        }
    }
}
