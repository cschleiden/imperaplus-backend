namespace ImperaPlus.Domain
{
    public interface IIdentifiableEntity<T>
    {
        T Id
        {
            get;
        }
    }

    public interface IIdentifiableEntity : IIdentifiableEntity<long>
    {
    }
}
