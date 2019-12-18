using nl.SWEG.RPGWizardry.GameWorld.OpacityManagement;
using nl.SWEG.RPGWizardry.Player.Inventory;
using nl.SWEG.RPGWizardry.Utils.Attributes;
using System;
using System.Linq;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Entities.Collectables
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
        private Vector2 opacityOffset;
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Called when Collectable is Collected
        /// </summary>
        /// <param name="target">Inventory-Target for Collection</param>
        protected abstract bool OnCollect(PlayerInventory target);

        /// <summary>
        /// Checks collision. Calls OnCollect, then Destroys GameObject if collision was valid
        /// </summary>
        /// <param name="collision">Collider with which Collision occured</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            print("picked up");

            PlayerInventory inv = collision.gameObject.GetComponent<PlayerInventory>();
            if (inv == null)
                throw new InvalidOperationException("Target has no Inventory");
            if (OnCollect(inv))
                Destroy(gameObject);
        }
        #endregion
    }
}