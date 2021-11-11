using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Enums;

namespace ImperaPlus.Domain.Bots
{
    public class Bot
    {
        private readonly ILogger log;
        private readonly Game game;
        private Team ownTeam;
        private Player ownPlayer;
        private MapTemplate mapTemplate;
        private IAttackService attackService;
        private IRandomGen randomGen;

        public Bot(
            ILogger log,
            Game game,
            MapTemplate mapTemplate,
            IAttackService attackService,
            IRandomGen randomGen)
        {
            this.log = log;
            this.game = game;
            this.mapTemplate = mapTemplate;
            this.attackService = attackService;
            this.randomGen = randomGen;
        }

        public void PlayTurn()
        {
            ownTeam = game.CurrentPlayer.Team;
            ownPlayer = game.CurrentPlayer;
            ;

            if (Place())
            {
                // Not first turn
                Attack();
                Move();
                EndTurn();
            }
        }

        private bool Place()
        {
            var unitsToPlace = game.GetUnitsToPlace(mapTemplate, ownPlayer);

            log.Log(LogLevel.Info, "[Bot] Placing {0} units", unitsToPlace);

            var ownCountries = game.Map.GetCountriesForTeam(ownTeam.Id);
            Country ownCountry;
            if (ownCountries.Count() == 1)
            {
                ownCountry = ownCountries.First();
            }
            else
            {
                ownCountry = ownCountries.FirstOrDefault(x =>
                    mapTemplate
                        .GetConnectedCountries(x.CountryIdentifier)
                        .Select(c => game.Map.GetCountry(c))
                        .Any(c => c.TeamId != ownTeam.Id));
            }

            if (ownCountry == null)
            {
                ownCountry = ownCountries.FirstOrDefault();

                if (ownCountry == null)
                {
                    log.Log(LogLevel.Error, "No connected, enemy country found");
                }
            }

            game.PlaceUnits(mapTemplate,
                new List<Tuple<string, int>> { Tuple.Create(ownCountry.CountryIdentifier, unitsToPlace) });

            return game.PlayState == PlayState.Attack;
        }

        private void Attack()
        {
            for (var a = 0; a < game.Options.AttacksPerTurn; ++a)
            {
                if (game.State != GameState.Active)
                {
                    // Probably won, don't try to attack.
                    break;
                }

                var ownCountries = game.Map.GetCountriesForTeam(ownTeam.Id);

                // Find own country, connected to an enemy one
                var ownCountry = ownCountries.FirstOrDefault(x =>
                    x.Units > game.Options.MinUnitsPerCountry
                    && game.Map.Countries.Any(y => y.TeamId != ownTeam.Id
                                                   && mapTemplate
                                                       .Connections
                                                       .Any(c => c.Origin == x.CountryIdentifier &&
                                                                 c.Destination == y.CountryIdentifier)));
                if (ownCountry == null)
                {
                    // Abort attack
                    break;
                }

                // Find enemy country
                var enemyCountries = game.Map.Countries.Where(x => x.TeamId != ownTeam.Id);
                var enemyCountry = enemyCountries.FirstOrDefault(x => mapTemplate
                    .Connections.Any(c =>
                        c.Origin == ownCountry.CountryIdentifier
                        && c.Destination == x.CountryIdentifier));
                if (enemyCountry == null)
                {
                    log.Log(LogLevel.Error, "Cannot find enemy country connected to selected own country");
                }

                var numberOfUnits = ownCountry.Units - game.Options.MinUnitsPerCountry;

                log.Log(
                    LogLevel.Info,
                    "Attack from {0} to {1} with {2} units",
                    ownCountry.CountryIdentifier,
                    enemyCountry.CountryIdentifier,
                    numberOfUnits);

                game.Attack(attackService, randomGen, mapTemplate, ownCountry.CountryIdentifier,
                    enemyCountry.CountryIdentifier, numberOfUnits);
            }
        }

        private void Move()
        {
            // Not supported.
        }

        private void EndTurn()
        {
            if (game.State == GameState.Active)
            {
                game.EndTurn();
            }
        }
    }
}
