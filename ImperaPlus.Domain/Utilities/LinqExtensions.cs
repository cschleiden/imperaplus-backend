using System;
using System.Collections.Generic;
using System.Linq;

namespace ImperaPlus.Domain.Utilities
{
    public static class LinqExtensions
    {
        public static T RandomElement<T>(this IEnumerable<T> source)
        {
            var rng = new Random();

            var count = source.Count();

            var idx = rng.Next(count);

            return source.Skip(idx).Take(1).FirstOrDefault();
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(
            this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (rng == null) throw new ArgumentNullException("rng");

            return source.ShuffleIterator(rng);
        }

        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, Random rng)
        {
            var buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}
