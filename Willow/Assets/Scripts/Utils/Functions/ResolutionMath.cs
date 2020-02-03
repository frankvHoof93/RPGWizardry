using UnityEngine;

namespace nl.SWEG.Willow.Utils.Functions
{
    /// <summary>
    /// Math for re-calculating positions based on Resolution
    /// </summary>
    public static class ResolutionMath
    {
        #region Variables
        /// <summary>
        /// Screen-Width Standard for ScreenSpace-Calculations
        /// </summary>
        public const float DefaultWidth = 1280;
        /// <summary>
        /// Screen-Height Standard for ScreenSpace-Calculations
        /// </summary>
        public const float DefaultHeight = 720;
        #endregion

        #region Methods
        /// <summary>
        /// Converts for current Resolution (squashed on axes)
        /// </summary>
        /// <param name="input">Vector to convert</param>
        /// <returns>Converted Vector (Both axes handles seperately)s</returns>
        public static Vector2 ConvertToResolution(Vector2 input)
        {
            return new Vector2(ConvertForWidth(input.x), ConvertForHeight(input.y));
        }
        /// <summary>
        /// Converts Width for current resolution
        /// </summary>
        /// <param name="input">Input-Value</param>
        /// <returns>Converted Value</returns>
        public static float ConvertForWidth(float input)
        {
            return input / (DefaultWidth / Screen.width);
        }
        /// <summary>
        /// Converts Height for current resolution
        /// </summary>
        /// <param name="input">Input-Value</param>
        /// <returns>Converted Value</returns>
        public static float ConvertForHeight(float input)
        {
            return input / (DefaultHeight / Screen.height);
        }
        #endregion
    }
}