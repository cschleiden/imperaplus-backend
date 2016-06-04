﻿using System.Collections.Generic;

namespace ImperaPlus.Domain.Map
{
    public class Continent : IIdentifiableEntity
    {
        private Continent()
        {            
        }

        public Continent(string name, int bonus)
        {
            this.Name = name;
            this.Bonus = bonus;
            this.Countries = new HashSet<CountryTemplate>();
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public int Bonus { get; set; }

        public virtual ICollection<CountryTemplate> Countries { get; private set; }
    }
}