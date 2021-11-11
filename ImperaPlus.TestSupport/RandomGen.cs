using System;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.TestSupport
{
    public class TestRandomGen : IRandomGen
    {
        private Random random = new();

        public int GetNext(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
