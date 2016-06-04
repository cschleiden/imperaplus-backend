using ImperaPlus.Application.Visibility;
using ImperaPlus.Domain.Services;
using ImperaPlus.DTO.Games;
using ImperaPlus.DTO.Games.History;
using ImperaPlus.DTO.Games.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.Application.Visibility
{
    public class FogVisibilityModifier : BaseVisibilityModifier
    {
        public FogVisibilityModifier(IMapTemplateProvider mapTemplateProvider)
            : base(mapTemplateProvider)
        {
        }

        public override void Apply(ImperaPlus.Domain.User user, Game game)
        {
            this.ApplyMap(user, game, game.Map);
        }

        private void ApplyMap(Domain.User user, Game game, Map map)
        {
            if (game.State == GameState.Ended)
            {
                return;
            }

            var visibleCountries = new List<Country>();

            var player = game.Teams.SelectMany(x => x.Players).FirstOrDefault(x => x.UserId == user.Id);
            if (player != null)
            {
                var team = game.Teams.First(x => x.Players.Any(p => p.Id == player.Id));

                var mapTemplate = this.MapTemplateProvider.GetTemplate(game.MapTemplate);
                var countryDict = map.Countries.ToDictionary(x => x.Identifier);

                foreach (var country in map.Countries)
                {
                    if (country.TeamId == team.Id // Country belongs to player's team
                    || mapTemplate
                            .GetConnectedCountries(country.Identifier)
                            .Select(x => countryDict[x])
                            .Any(x => x.TeamId == team.Id)) // Country is connected to a country which belongs to player's team
                    {
                        visibleCountries.Add(country);
                    }
                }
            }

            map.Countries = visibleCountries.ToArray();
        }

        public override void Apply(ImperaPlus.Domain.User user, HistoryTurn historyTurn, DTO.Games.Map.Map previousMap)
        {
            if (historyTurn.Game.State == GameState.Ended)
            {
                return;
            }

            // Apply to current turn
            this.Apply(user, historyTurn.Game);

            // Apply to previous turn map
            this.ApplyMap(user, historyTurn.Game, previousMap);

            var visibleCountries = new HashSet<string>(previousMap.Countries.Select(x => x.Identifier));

            var visibleActions = new List<HistoryEntry>();
            foreach(var action in historyTurn.Actions.ToArray())
            {
                // Check that at least origin or destination of each action is visible, or no country is involved
                if ((action.OriginIdentifier == null && action.DestinationIdentifier == null)
                    || visibleCountries.Contains(action.OriginIdentifier)
                    || visibleCountries.Contains(action.DestinationIdentifier))
                {
                    visibleActions.Add(action);
                }
            }

            historyTurn.Actions = visibleActions.ToArray();
        }

        public override void Expand(Domain.User user, Domain.Games.Game game, List<Domain.Games.Country> changedCountries)
        {
            var player = game.Teams.SelectMany(x => x.Players).FirstOrDefault(x => x.UserId == user.Id);
            if (player != null)
            {
                HashSet<Domain.Games.Country> countries = new HashSet<Domain.Games.Country>(changedCountries);
                HashSet<Domain.Games.Country> revealedCountries = new HashSet<Domain.Games.Country>();

                var team = player.Team;

                var mapTemplate = this.MapTemplateProvider.GetTemplate(game.MapTemplateName);

                foreach (var changedCountry in changedCountries)
                {
                    if (changedCountry.TeamId != team.Id)
                    {
                        // Country does not belong to team, do not expand
                        continue;
                    }

                    // Add connected countries
                    foreach(var connectedCountry in mapTemplate
                            .GetConnectedCountries(changedCountry.CountryIdentifier)
                            .Select(x => game.Map.GetCountry(x)))
                    {
                        if (!revealedCountries.Contains(connectedCountry)
                            && !countries.Contains(connectedCountry))
                        {
                            revealedCountries.Add(connectedCountry);
                        }
                    }
                }

                changedCountries.AddRange(revealedCountries);
            }
        }
    }
}