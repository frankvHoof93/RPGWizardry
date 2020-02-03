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
                throw new ArgumentNullException("array");

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
    }
}