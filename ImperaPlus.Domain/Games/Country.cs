using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Utilities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ImperaPlus.Domain.Games
{
    public class Country
    {
        private int units;

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

        public Guid PlayerId { get; internal set; }

        public Guid TeamId { get; internal set; }

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
       
        public string CountryIdentifier { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this country has been updated in the current turn
        /// </summary>
        [IgnoreDataMember]        
        public bool IsUpdated { get; internal set; }

        public static Country CreateFromTemplate(Map map, CountryTemplate countryTemplate, int units)
        {
            return new Country(countryTemplate.Identifier, units);
        }

        public void PlaceUnits(int units)
        {
            if (units <= 0)
            {
                throw new DomainException(ErrorCode.ZeroNegativeUnits, "Cannot place zero or negative units");
            }

            this.Units += units;
        }

        public bool IsNeutral
        {
            get
            {
                return this.PlayerId == Guid.Empty;
            }
        }
    }
}