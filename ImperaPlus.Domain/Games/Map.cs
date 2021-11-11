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
                if (playerToCountry == null)
                {
                    playerToCountry = Countries.GroupBy(x => x.PlayerId)
                        .ToDictionary(x => x.Key, x => x.ToList());
                }

                return playerToCountry;
            }
        }

        protected Map()
        {
            Countries = new List<Country>();
        }

        internal Map(Game game)
            : this()
        {
            Game = game;
        }

        public long Id { get; set; }

        // Backing field
        public string SerializedCountries
        {
            get => countriesSerialized;

            set
            {
                countriesSerialized = value;
                Countries = Jil.JSON.Deserialize<List<Country>>(countriesSerialized);
                ResetTracking();
            }
        }

        public long GameId { get; set; }

        public virtual Game Game { get; set; }

        [NotMapped] public IList<Country> Countries { get; private set; }

        [NotMapped] public IEnumerable<Country> ChangedCountries => Countries.Where(x => x.IsUpdated);

        public Map Clone()
        {
            return new Map(Game)
            {
                Countries = Countries.Select(country => new Country(country.CountryIdentifier, country.Units)
                {
                    PlayerId = country.PlayerId, TeamId = country.TeamId, IsUpdated = false
                }).ToList()
            };
        }

        /// <summary>
        /// Reset country change tracking
        /// </summary>
        public void ResetTracking()
        {
            foreach (var country in Countries)
            {
                country.IsUpdated = false;
            }
        }

        public Country GetCountry(string identifier)
        {
            if (countryDict == null)
            {
                countryDict = Countries.ToDictionary(x => x.CountryIdentifier);
            }

            Country country;
            if (!countryDict.TryGetValue(identifier, out country))
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

        public void Distribute(GameOptions gameOptions, ICollection<Team> teams, MapTemplate mapTemplate,
            MapDistribution mapDistribution, IRandomGen random)
        {
            var distribution = MapDistributionFactory.Create(mapDistribution);
            distribution.Distribute(gameOptions, teams, mapTemplate, this, random);
        }

        public IEnumerable<Country> GetCountriesForPlayer(Player player)
        {
            return GetCountriesForPlayer(player.Id);
        }

        public IEnumerable<Country> GetCountriesForPlayer(Guid playerId)
        {
            if (PlayerToCountry.ContainsKey(playerId))
            {
                return PlayerToCountry[playerId];
            }

            return Enumerable.Empty<Country>();
        }

        public IEnumerable<Country> GetCountriesForTeam(Guid teamId)
        {
            var playerIds = Game
                .Teams.Single(x => x.Id == teamId)
                .Players.Select(p => p.Id);

            return Countries.Where(c => playerIds.Contains(c.PlayerId));
        }

        internal void UpdateOwnership(Player oldPlayer, Player newPlayer, Country country)
        {
            if (oldPlayer == null)
            {
                // Try to determine player from current map state
                oldPlayer = Game.GetPlayerById(country.PlayerId);
            }

            if (oldPlayer != null)
            {
                if (PlayerToCountry.ContainsKey(oldPlayer.Id))
                {
                    PlayerToCountry[oldPlayer.Id].Remove(country);
                }
            }

            if (newPlayer != null)
            {
                if (PlayerToCountry.ContainsKey(newPlayer.Id))
                {
                    PlayerToCountry[newPlayer.Id].Add(country);
                }
                else
                {
                    PlayerToCountry.Add(newPlayer.Id, new List<Country> { country });
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
            UpdateOwnership(null, player, country);
        }

        private void ResetChanges()
        {
            foreach (var country in ChangedCountries.ToArray())
            {
                country.IsUpdated = false;
            }
        }

        public void Serialize()
        {
            countriesSerialized = Jil.JSON.Serialize(Countries);
        }
    }
}
