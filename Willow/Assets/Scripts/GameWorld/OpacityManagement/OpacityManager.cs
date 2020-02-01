using nl.SWEG.Willow.GameWorld.OpacityManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nl.SWEG.Willow.GameWorld
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class OpacityManager : MonoBehaviour
    {
        #region InnerObjects
        /// <summary>
        /// Object used to store Transform with its IOpacity-Implementation
        /// </summary>
        protected class OpacityObject
        {
            public Transform transform;
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
        /// Checks if the thing entering the trigger is a player, and if it's fhe first thing to enter it, makes the walls transparent.
        /// </summary>
        /// <param name="collision">The thing entering the collider.</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            IOpacity opacity = collision.gameObject.GetComponent<IOpacity>();
            if (opacity != null)
                objects.Add(new OpacityObject { transform = collision.transform, opacity = opacity });
        }

        /// <summary>
        /// Check if it's the last thing leaving the trigger, and if so, makes walls opague.
        /// </summary>
        /// <param name="collision">the thing leaving the collider.</param>
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