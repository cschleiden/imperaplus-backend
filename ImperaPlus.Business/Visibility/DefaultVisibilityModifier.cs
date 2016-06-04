using ImperaPlus.Domain.Services;
using ImperaPlus.DTO.Games;
using ImperaPlus.DTO.Games.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.Application.Visibility
{
    public class DefaultVisibilityModifier : BaseVisibilityModifier
    {
        public DefaultVisibilityModifier(IMapTemplateProvider mapTemplateProvider)
            : base(mapTemplateProvider)
        {
        }

        public override void Apply(ImperaPlus.Domain.User user, Game game)
        {
            // Do not modify anything
        }

        public override void Apply(ImperaPlus.Domain.User user, HistoryTurn historyTurn, DTO.Games.Map.Map previousHistoryTurn)
        {
            // Do not modify anything
        }

        public override void Expand(Domain.User user, Domain.Games.Game game, List<Domain.Games.Country> changedCountries)
        {
            // Do not modify anything
        }
    }
}
