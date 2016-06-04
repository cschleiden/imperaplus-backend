namespace ImperaPlus.Domain
{
    public interface IIdentifiableEntity<T>
    {
        T Id
        {
            get;
            set;
        }
    }

    public interface IIdentifiableEntity : IIdentifiableEntity<long>
    {        
    }
}
