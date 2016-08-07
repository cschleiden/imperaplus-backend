namespace ImperaPlus.Domain.Map
{
    public class CountryTemplate
    {
        private CountryTemplate()
        {            
        }

        public CountryTemplate(string identifier, string name)
        {
            this.Identifier = identifier;
            this.Name = name;
        }

        public long Id { get; set; }

        public string Identifier { get; set; }

        public string Name { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}