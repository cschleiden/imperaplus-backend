namespace ImperaPlus.Domain.Map
{
    public class Connection : IIdentifiableEntity
    {
        private Connection()
        {            
        }

        public Connection(string origin, string destination)
        {
            this.Origin = origin;
            this.Destination = destination;
        }

        public long Id { get; set; }

        public virtual string Origin { get; private set; }
                       
        public virtual string Destination { get; private set; }
    }
}