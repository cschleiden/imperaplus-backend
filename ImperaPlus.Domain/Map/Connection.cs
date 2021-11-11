namespace ImperaPlus.Domain.Map
{
    public class Connection : IIdentifiableEntity
    {
        private Connection()
        {
        }

        public Connection(string origin, string destination)
        {
            Origin = origin;
            Destination = destination;
        }

        public long Id { get; set; }

        public virtual string Origin { get; private set; }

        public virtual string Destination { get; private set; }
    }
}
