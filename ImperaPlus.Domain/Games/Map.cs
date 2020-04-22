using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Games.Distribution;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Domain.Games
{
    public class Map : Entity, ISerializedEntity
    {
        private Dictionary<string, Country> countryDict;
        private Dictionary<Guid, List<Country>> playerToCountry = null;

        // Backing property
        private string countriesSerialized;

        protected IDictionary<Guid, List<Country>> PlayerToCountry
        {
            get
            {
                if (this.playerToCountry == null)
                {
                    this.playerToCountry = this.Countries.GroupBy(x => x.PlayerId)
                                                            .ToDictionary(x => x.Key, x => x.ToList());
                }

                return this.playerToCountry;
            }
        }

        protected Map()
        {
            this.Countries = new List<Country>();
        }

        internal Map(Game game)
            : this()
        {
            this.Game = game;
        }

        public long Id { get; set; }

        // Backing field
        public string SerializedCountries
        {
            get
            {
                return this.countriesSerialized;
            }

            set
            {
                this.countriesSerialized = value;
                this.Countries = Jil.JSON.Deserialize<List<Country>>(this.countriesSerialized);
                this.ResetTracking();
            }
        }

        public long GameId { get; set; }

        public virtual Game Game { get; set;}

        [NotMapped]
        public IList<Country> Countries { get; private set; }

        [NotMapped]
        public IEnumerable<Country> ChangedCountries
        {
            get
            {
                return this.Countries.Where(x => x.IsUpdated);
            }
        }

        public Map Clone()
        {
            return new Map(this.Game)
            {
                Countries = this.Countries.Select(country => new Country(country.CountryIdentifier, country.Units)
                {
                    PlayerId = country.PlayerId,
                    TeamId = country.TeamId,
                    IsUpdated = country.IsUpdated
                }).ToList()
            };
        }

        /// <summary>
        /// Reset country change tracking
        /// </summary>
        public void ResetTracking()
        {
            foreach(var country in this.Countries)
            {
                country.IsUpdated = false;
            }
        }

        public Country GetCountry(string identifier)
        {
            if (this.countryDict == null)
            {
                this.countryDict = this.Countries.ToDictionary(x => x.CountryIdentifier);
            }

            Country country;
            if (!this.countryDict.TryGetValue(identifier, out country))
            {
                throw new DomainException(ErrorCode.CountryNotFound, "Cannot find country");
            }

            return country;
        }

        public static Map CreateFromTemplate(Game game, MapTemplate mapTemplate)
        {
            var map = new Map(game);

            foreach (var countryTemplate in mapTemplate.Countries)
            {
                var country = Country.CreateFromTemplate(map, countryTemplate, game.Options.InitialCountryUnits);

                map.Countries.Add(country);
            }

            return map;
        }

        public void Distribute(ICollection<Team> teams, MapTemplate mapTemplate, MapDistribution mapDistribution, IRandomGen random)
        {
            var distribution = MapDistributionFactory.Create(mapDistribution);
            distribution.Distribute(teams, mapTemplate, this, random);
        }

        public IEnumerable<Country> GetCountriesForPlayer(Player player)
        {
            return this.GetCountriesForPlayer(player.Id);
        }

        public IEnumerable<Country> GetCountriesForPlayer(Guid playerId)
        {
            if (this.PlayerToCountry.ContainsKey(playerId))
            {
                return this.PlayerToCountry[playerId];
            }

            return Enumerable.Empty<Country>();
        }

        public IEnumerable<Country> GetCountriesForTeam(Guid teamId)
        {
            var playerIds = this.Game
                .Teams.Single(x => x.Id == teamId)
                .Players.Select(p => p.Id);

            return this.Countries.Where(c => playerIds.Contains(c.PlayerId));
        }

        internal void UpdateOwnership(Player oldPlayer, Player newPlayer, Country country)
        {
            if (oldPlayer == null)
            {
                // Try to determine player from current map state
                oldPlayer = this.Game.GetPlayerById(country.PlayerId);
            }

            if (oldPlayer != null)
            {
                if (this.PlayerToCountry.ContainsKey(oldPlayer.Id))
                {
                    this.PlayerToCountry[oldPlayer.Id].Remove(country);
                }
            }

            if (newPlayer != null)
            {
                if (this.PlayerToCountry.ContainsKey(newPlayer.Id))
                {
                    this.PlayerToCountry[newPlayer.Id].Add(country);
                }
                else
                {
                    this.PlayerToCountry.Add(newPlayer.Id, new List<Country> { country });
                }

                country.PlayerId = newPlayer.Id;
                country.TeamId = newPlayer.TeamId;
            }
            else
            {
                country.PlayerId = Guid.Empty;
                country.TeamId = Guid.Empty;
            }
        }

        internal void UpdateOwnership(Player player, Country country)
        {
            this.UpdateOwnership(null, player, country);
        }

        private void ResetChanges()
        {
            foreach(var country in this.ChangedCountries.ToArray())
            {
                country.IsUpdated = false;
            }
        }

        public void Serialize()
        {
            this.countriesSerialized = Jil.JSON.Serialize(this.Countries);
        }
    }
}
