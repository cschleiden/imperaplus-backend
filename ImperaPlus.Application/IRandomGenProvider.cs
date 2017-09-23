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
        private Object sync = new object();
        private RNGCryptoServiceProvider random;

        public RandomGenProvider()
        {
            this.random = new RNGCryptoServiceProvider();
        }

        public IRandomGen GetRandomGen()
        {
            byte[] buffer = new byte[4];

            lock (this.sync)
            {
                this.random.GetBytes(buffer);
            }

            return new RandomGen(BitConverter.ToInt32(buffer, 0));
        }
    }
}
