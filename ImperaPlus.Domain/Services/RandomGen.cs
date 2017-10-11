namespace ImperaPlus.Domain.Services
{
    public interface IRandomGen
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">Inclusive lower bound</param>
        /// <param name="max">Inclusive upper bound</param>
        /// <returns></returns>
        int GetNext(int min, int max);
    }
}
