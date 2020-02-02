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
            public Transform transform;
            /// <summary>
            /// Settings for Opacity
            /// </summary>
            public IOpacity opacity;
        }
        #endregion

        #region Variables
        /// <summary>
        /// Renderers to apply Opacity to
        /// </summary>
        [SerializeField]
        [Tooltip("Renderers to apply Opacity to")]
        protected Renderer[] renderers;
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
                objects.Add(new OpacityObject { transform = collision.transform, opacity = opacity });
        }

        /// <summary>
        /// Removes Opacity-Objects from list when they leave the collider
        /// </summary>
        /// <param name="collision">Collider for Object leaving trigger</param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            objects.RemoveWhere(n => ReferenceEquals(n.transform, collision.transform));
        }

        /// <summary>
        /// Sets Opacity to Renderer
        /// </summary>
        private void LateUpdate()
        {
            objects.RemoveWhere(o => o == null || o.transform == null);
            SetToShader(objects.OrderBy(n => n.opacity.OpacityPriority).ToList());
        }

        /// <summary>
        /// Sets Opacity to Material/Shader
        /// </summary>
        /// <param name="objects">Objects to set Opacity for</param>
        protected abstract void SetToShader(List<OpacityObject> objects);
        #endregion
    }
}