using UnityEngine;

namespace nl.SWEG.Willow.Utils.Functions
{
    /// <summary>
    /// Extension-Functions for Unity LayerMask
    /// </summary>
    public static class LayerMaskExtensions
    {
        /// <summary>
        /// Checks if a Layer is in a LayerMask
        /// </summary>
        /// <param name="mask">Mask to check in</param>
        /// <param name="layer">Layer to check for</param>
        /// <returns>True if LayerMask contains Layer</returns>
        public static bool HasLayer(this LayerMask mask, int layer)
        {
            return mask == (mask | 1 << layer);
        }
    }
}
