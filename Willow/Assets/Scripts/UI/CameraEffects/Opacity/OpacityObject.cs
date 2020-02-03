using UnityEngine;

namespace nl.SWEG.Willow.UI.CameraEffects.Opacity
{
    /// <summary>
    /// Object that turns specific parts of the environment transparent when it is behind them
    /// </summary>
    public class OpacityObject : MonoBehaviour, IOpacity
    {
        #region Public
        /// <summary>
        /// Radius for Opacity-Circle
        /// </summary>
        public float OpacityRadius => opacityRadius;
        /// <summary>
        /// Priority for Object-Opacity when rendering small batches
        /// </summary>
        public int OpacityPriority => opacityPriority;
        /// <summary>
        /// Offset for Opacity-Circle (from Transform, in WorldSpace)
        /// </summary>
        public Vector2 OpacityOffset => opacityOffset;
        #endregion

        #region Editor
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Radius for Opacity-Circle
        /// </summary>
        [SerializeField]
        [Tooltip("Radius for Opacity-Circle")]
        private float opacityRadius;
        /// <summary>
        /// Priority for Object-Opacity when rendering small batches
        /// </summary>
        [SerializeField]
        [Tooltip("Priority for Object-Opacity when rendering small batches")]
        private int opacityPriority = 1;
        /// <summary>
        /// Offset for Opacity-Circle (from Transform, in WorldSpace)
        /// </summary>
        [SerializeField]
        [Tooltip("Offset for Opacity-Circle (from Transform, in WorldSpace)")]
        private Vector2 opacityOffset;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion
    }
}