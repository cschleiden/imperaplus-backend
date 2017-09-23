namespace ImperaPlus.Domain.Services
{
    public interface IRandomGen
    {
        int GetNext(int min, int max);
    }
}
