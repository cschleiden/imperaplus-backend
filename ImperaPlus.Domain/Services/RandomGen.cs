using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.Domain.Services
{
    public interface IRandomGen
    {
        int GetNext(int min, int max);
    }

    public class RandomGen : IRandomGen
    {
        private Random random = new Random();

        public int GetNext(int min, int max)
        {
            return this.random.Next(min, max + 1);
        }
    }
}
