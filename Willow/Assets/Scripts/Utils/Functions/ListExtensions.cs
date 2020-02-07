using System;
using System.Collections.Generic;
using System.Linq;

namespace nl.SWEG.Willow.Utils.Functions
{
    /// <summary>
    /// Extension-Functions for Lists
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Chunks List by size
        /// </summary>
        /// <typeparam name="T">Type of Object in List</typeparam>
        /// <param name="source">Source-List to Chunk</param>
        /// <param name="chunkSize">Size for each chunk</param>
        /// <returns>List of Chunks</returns>
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        /// <summary>
        /// Shuffles Objects in List
        /// </summary>
        /// <typeparam name="T">Type of Object in List</typeparam>
        /// <param name="list">List to Shuffle</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random(list.GetHashCode());
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
