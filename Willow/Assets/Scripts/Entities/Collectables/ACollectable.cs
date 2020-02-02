using nl.SWEG.Willow.Player.Inventory;
using nl.SWEG.Willow.UI.CameraEffects.Opacity;
using UnityEngine;

namespace nl.SWEG.Willow.Entities.Collectables
{
    /// <summary>
    /// Base Class for a Collectable Object
    /// </summary>
    public abstract class ACollectable : MonoBehaviour, IOpacity
    {
        #region Variables
        #region Public
        /// <summary>
        /// Priority for rendering Opacity
        /// </summary>
        public int OpacityPriority => 100; // Low(est) priority
        /// <summary>
        /// Opacity-Radius in Pixels (for 720p)
        /// </summary>
        public float OpacityRadius => opacityRadius;
        /// <summary>
        /// Opacity-Offset from Transform (in World-Space)
        /// </summary>
        public Vector2 OpacityOffset => opacityOffset;
        #endregion

        #region Editor
        [Header("Opacity")]
        /// <summary>
        /// Opacity-Radius in Pixels (for 720p)
        /// </summary>
        [SerializeField]
        [Tooltip("Opacity-Radius in Pixels (for 720p)")]
        private float opacityRadius = 40f;
        /// <summary>
        /// Opacity-Offset from Transform (in World-Space)
        /// </summary>
        [SerializeField]
        [Tooltip("Opacity-Offset from Transform (in World-Space)")]
        private Vector2 opacityOffset = Vector2.zero;
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Called when Collectable is Collected
        /// </summary>
        /// <param name="target">Inventory-Target for Collection</param>
        /// <returns>True if Collection was successful</returns>
        protected abstract bool OnCollect(PlayerInventory target);

        /// <summary>
        /// Checks collision. Calls OnCollect, then Destroys GameObject if collision was valid
        /// </summary>
        /// <param name="collision">Collider with which Collision occured</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            PlayerInventory inv = collision.gameObject.GetComponent<PlayerInventory>();
            if (inv == null)
                return;
            if (OnCollect(inv))
                Destroy(gameObject);
        }
        #endregion
    }
}