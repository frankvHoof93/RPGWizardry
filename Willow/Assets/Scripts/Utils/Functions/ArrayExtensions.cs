using System;
using System.Runtime.InteropServices;

namespace nl.SWEG.Willow.Utils.Functions
{
    /// <summary>
    /// Extension-Functions for Arrays
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Retrieves a Row from an Array
        /// </summary>
        /// <typeparam name="T">Type of Object in Array (Must be Primitive)</typeparam>
        /// <param name="array">Source-Array to retrieve row from</param>
        /// <param name="row">Index for row to retrieve</param>
        /// <returns>Single row from Array</returns>
        public static T[] GetRow<T>(this T[,] array, int row)
        {
            if (!typeof(T).IsPrimitive)
                throw new InvalidOperationException("Not supported for managed types.");

            if (array == null)
                throw new ArgumentNullException(nameof(array));

            int cols = array.GetUpperBound(1) + 1;
            T[] result = new T[cols];

            int size;

            if (typeof(T) == typeof(bool))
                size = 1;
            else if (typeof(T) == typeof(char))
                size = 2;
            else
                size = Marshal.SizeOf<T>();

            Buffer.BlockCopy(array, row * cols * size, result, 0, cols * size);

            return result;
        }
        /// <summary>
        /// Shuffles Objects in Array, returning an array of indices for the original order
        /// </summary>
        /// <typeparam name="T">Type of Objects in Array</typeparam>
        /// <param name="array">Array to Shuffle</param>
        /// <returns>Array of indices, which correspond to the index of an object before the shuffle</returns>
        public static int[] Shuffle<T>(this T[] array)
        {
            int[] indices = new int[array.Length];
            for (int i = 0; i < indices.Length; i++)
                indices[i] = i;
            Random rng = new Random(array.GetHashCode());
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
                int index = indices[k];
                indices[k] = indices[n];
                indices[n] = index;
            }
            return indices;
        }
    }
}