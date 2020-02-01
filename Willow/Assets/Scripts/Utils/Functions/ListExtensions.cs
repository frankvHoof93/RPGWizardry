using System.Collections.Generic;
using System.Linq;

namespace nl.SWEG.Willow.Utils
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
    }
}
