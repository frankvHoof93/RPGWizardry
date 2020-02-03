using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nl.SWEG.Willow.UI.CameraEffects.Opacity
{
    /// <summary>
    /// Manages Opacity for Objects
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public abstract class OpacityManager : MonoBehaviour
    {
        #region InnerObjects
        /// <summary>
        /// Object used to store Transform with its IOpacity-Implementation
        /// </summary>
        protected class OpacityObject
        {
            /// <summary>
            /// Transform to Position Opacity around
            /// </summary>
            public Transform Transform;
            /// <summary>
            /// Settings for Opacity
            /// </summary>
            public IOpacity Opacity;
        }
        #endregion

        #region Variables
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Renderers to apply Opacity to
        /// </summary>
        [SerializeField]
        [Tooltip("Renderers to apply Opacity to")]
        protected Renderer[] renderers;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        /// <summary>
        /// (Current) Objects to get Opacity from
        /// </summary>
        private readonly HashSet<OpacityObject> objects = new HashSet<OpacityObject>();
        #endregion

        #region Methods
        /// <summary>
        /// Adds Opacity-Objects to list when they enter the collider
        /// </summary>
        /// <param name="collision">Collider with which collision occurred</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            IOpacity opacity = collision.gameObject.GetComponent<IOpacity>();
            if (opacity != null)
                objects.Add(new OpacityObject { Transform = collision.transform, Opacity = opacity });
        }

        /// <summary>
        /// Removes Opacity-Objects from list when they leave the collider
        /// </summary>
        /// <param name="collision">Collider for Object leaving trigger</param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            objects.RemoveWhere(n => ReferenceEquals(n.Transform, collision.transform));
        }

        /// <summary>
        /// Sets Opacity to Renderer
        /// </summary>
        private void LateUpdate()
        {
            objects.RemoveWhere(o => o == null || o.Transform == null);
            SetToShader(objects.OrderBy(n => n.Opacity.OpacityPriority).ToList());
        }

        /// <summary>
        /// Sets Opacity to Material/Shader
        /// </summary>
        /// <param name="objects">Objects to set Opacity for</param>
        protected abstract void SetToShader(List<OpacityObject> objects);
        #endregion
    }
}