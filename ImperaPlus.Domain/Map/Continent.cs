using System.Collections.Generic;

namespace ImperaPlus.Domain.Map
{
    public class Continent
    {
        private Continent()
        {
        }

        public Continent(string name, int bonus)
        {
            Name = name;
            Bonus = bonus;
            Countries = new HashSet<CountryTemplate>();
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public int Bonus { get; set; }

        public virtual ICollection<CountryTemplate> Countries { get; private set; }
    }
}
