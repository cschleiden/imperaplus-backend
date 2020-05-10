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
            this.IsUpdated = false;
        }

        public Country(string countryIdentifier, int units)
            : this()
        {
            this.CountryIdentifier = countryIdentifier;
            this.units = units;
        }

        public Guid PlayerId
        {
            get
            {
                return this.playerId;
            }

            internal set
            {
                if (this.playerId != value)
                {
                    // Capitals fall when the ownership changes.
                    this.Flags &= ~CountryFlags.Capital;

                    this.IsUpdated = true;
                }

                this.playerId = value;
            }
        }

        public Guid TeamId
        {
            get
            {
                return this.teamId;
            }

            internal set
            {
                if (this.teamId != value)
                {
                    this.IsUpdated = true;
                }

                this.teamId = value;
            }
        }

        public int Units
        {
            get
            {
                return this.units;
            }

            set
            {
                if (this.units != value)
                {
                    this.units = value;
                    this.IsUpdated = true;
                }
            }
        }

        public CountryFlags Flags
        {
            get => this.flags;
            set
            {
                if (this.flags != value)
                {
                    this.flags = value;
                    this.IsUpdated = true;
                }
            }
        }

        [NotMapped]
        public bool IsNeutral
        {
            get
            {
                return this.PlayerId == Guid.Empty;
            }
        }

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

            this.Units += units;
        }
    }
}