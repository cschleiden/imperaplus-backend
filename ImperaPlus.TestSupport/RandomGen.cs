using System;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.TestSupport
{
    public class RandomGen : IRandomGen
    {
        private Random random = new Random();

        public int GetNext(int min, int max)
        {
            return this.random.Next(min, max);
        }
    }
}
