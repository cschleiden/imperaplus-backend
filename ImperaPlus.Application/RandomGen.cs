using System;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Application
{
    public class RandomGen : IRandomGen
    {
        private Random random;

        public RandomGen(int seed)
        {
            this.random = new Random(seed);
        }

        public int GetNext(int min, int max)
        {
            return this.random.Next(min, max + 1);
        }
    }
}