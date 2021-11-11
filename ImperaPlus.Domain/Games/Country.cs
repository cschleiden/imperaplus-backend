using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Map;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ImperaPlus.Domain.Games
{
    public class Country
    {
        public static Country CreateFromTemplate(Map map, CountryTemplate countryTemplate, int units)
        {
            return new Country(countryTemplate.Identifier, units);
        }

        private CountryFlags flags;

        private int units;

        private Guid playerId;

        private Guid teamId;

        protected Country()
        {
            IsUpdated = false;
        }

        public Country(string countryIdentifier, int units)
            : this()
        {
            CountryIdentifier = countryIdentifier;
            this.units = units;
        }

        public Guid PlayerId
        {
            get => playerId;

            internal set
            {
                if (playerId != value)
                {
                    // Capitals fall when the ownership changes.
                    Flags &= ~CountryFlags.Capital;

                    IsUpdated = true;
                }

                playerId = value;
            }
        }

        public Guid TeamId
        {
            get => teamId;

            internal set
            {
                if (teamId != value)
                {
                    IsUpdated = true;
                }

                teamId = value;
            }
        }

        public int Units
        {
            get => units;

            set
            {
                if (units != value)
                {
                    units = value;
                    IsUpdated = true;
                }
            }
        }

        public CountryFlags Flags
        {
            get => flags;
            set
            {
                if (flags != value)
                {
                    flags = value;
                    IsUpdated = true;
                }
            }
        }

        [NotMapped] public bool IsNeutral => PlayerId == Guid.Empty;

        public string CountryIdentifier { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this country has been updated in the current turn
        /// </summary>
        [NotMapped]
        public bool IsUpdated { get; internal set; }

        public void PlaceUnits(int units)
        {
            if (units <= 0)
            {
                throw new DomainException(ErrorCode.ZeroNegativeUnits, "Cannot place zero or negative units");
            }

            Units += units;
        }
    }
}
