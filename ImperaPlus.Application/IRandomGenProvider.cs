using System;
using System.Security.Cryptography;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Application
{
    public interface IRandomGenProvider
    {
        IRandomGen GetRandomGen();
    }

    public class RandomGenProvider : IRandomGenProvider
    {
        private object sync = new();
        private RNGCryptoServiceProvider random;

        public RandomGenProvider()
        {
            random = new RNGCryptoServiceProvider();
        }

        public IRandomGen GetRandomGen()
        {
            var buffer = new byte[4];

            lock (sync)
            {
                random.GetBytes(buffer);
            }

            return new RandomGen(BitConverter.ToInt32(buffer, 0));
        }
    }
}
