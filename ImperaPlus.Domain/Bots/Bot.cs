using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Map;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Domain.Bots
{
    public class Bot
    {
        private readonly Game game;
        private Team ownTeam;
        private Player ownPlayer;
        private MapTemplate mapTemplate;
        private IAttackService attackService;
        private IRandomGen randomGen;

        public Bot(
            Game game, 
            MapTemplate mapTemplate,
            IAttackService attackService,
            IRandomGen randomGen)
        {
            this.game = game;
            this.mapTemplate = mapTemplate;
            this.attackService = attackService;
            this.randomGen = randomGen;
        }

        public void PlayTurn()
        {
            this.ownTeam = game.CurrentPlayer.Team;
            this.ownPlayer = game.CurrentPlayer;;

            if (this.Place())
            {
                // Not first turn
                this.Attack();
                this.Move();
                this.EndTurn();
            }
        }

        private bool Place()
        {
            var unitsToPlace = this.game.GetUnitsToPlace(this.mapTemplate, this.ownPlayer);

            Log.Debug().Message("[Bot] Placing {0} units", unitsToPlace).Write();
            var ownCountries = this.game.Map.GetCountriesForTeam(this.ownTeam.Id);
            Country ownCountry;
            if (ownCountries.Count() == 1)
            {
                ownCountry = ownCountries.First();
            }
            else
            {
                ownCountry = ownCountries.FirstOrDefault(x =>
                    this.mapTemplate
                        .GetConnectedCountries(x.CountryIdentifier)
                        .Select(c => this.game.Map.GetCountry(c))
                        .Any(c => c.TeamId != this.ownTeam.Id));
            }

            if (ownCountry == null)
            {
                ownCountry = ownCountries.FirstOrDefault();

                if (ownCountry == null)
                {
                    Log.Fatal().Message("No connected, enemy country found").Write();
                }
            }

            this.game.PlaceUnits(this.mapTemplate, new List<Tuple<string, int>> { Tuple.Create(ownCountry.CountryIdentifier, unitsToPlace) });

            return this.game.PlayState == Enums.PlayState.Attack;
        }

        private void Attack()
        {
            for (int a = 0; a < this.game.Options.AttacksPerTurn; ++a)
            {
                var ownCountries = this.game.Map.GetCountriesForTeam(this.ownTeam.Id);
                
                // Find own country, connected to an enemy one
                var ownCountry = ownCountries.FirstOrDefault(x =>
                    x.Units > this.game.Options.MinUnitsPerCountry
                    && this.game.Map.Countries.Any(y => y.TeamId != this.ownTeam.Id
                                        && mapTemplate
                                        .Connections
                                        .Any(c => c.Origin == x.CountryIdentifier && c.Destination == y.CountryIdentifier)));
                if (ownCountry == null)
                {
                    // Abort attack
                    break;
                }

                // Find enemy country
                var enemyCountries = this.game.Map.Countries.Where(x => x.TeamId != this.ownTeam.Id);
                var enemyCountry = enemyCountries.FirstOrDefault(x => mapTemplate
                                        .Connections.Any(c =>
                                        c.Origin == ownCountry.CountryIdentifier
                                        && c.Destination == x.CountryIdentifier));
                if (enemyCountry == null)
                {
                    Log.Fatal().Message("Cannot find enemy country connected to selected own country").Write();
                }

                var numberOfUnits = ownCountry.Units - this.game.Options.MinUnitsPerCountry;

                Log.Debug().Message("Attack from {0} to {1} with {2} units",
                    ownCountry.CountryIdentifier,
                    enemyCountry.CountryIdentifier,
                    numberOfUnits).Write();

                this.game.Attack(this.attackService, this.randomGen, this.mapTemplate, ownCountry.CountryIdentifier, enemyCountry.CountryIdentifier, numberOfUnits);
            }
        }

        private void Move()
        {
            // TODO: CS:
        }

        private void EndTurn()
        {
            this.game.EndTurn();
        }
    }
}