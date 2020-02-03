using UnityEngine;

namespace nl.SWEG.Willow.UI.CameraEffects.Opacity
{
    /// <summary>
    /// Interface for handling Opacity on Objects
    /// </summary>
    public interface IOpacity
    {
        /// <summary>
        /// Radius for Opacity (in Pixels)
        /// </summary>
        float OpacityRadius { get; }
        /// <summary>
        /// Priority for Opacity. If there are too many Objects to handle, this priority is used
        /// </summary>
        int OpacityPriority { get; }
        /// <summary>
        /// Offset (in WorldSpace) from Transform for Opacity
        /// </summary>
        Vector2 OpacityOffset { get; }
    }
}