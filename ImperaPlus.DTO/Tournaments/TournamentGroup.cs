using System;
using System.Collections;
using System.Collections.Generic;

namespace ImperaPlus.DTO.Tournaments
{
    public class TournamentGroup
    {
        public Guid Id { get; set; }


        public IEnumerable<TournamentTeamSummary> Teams { get; set; }
    }
}