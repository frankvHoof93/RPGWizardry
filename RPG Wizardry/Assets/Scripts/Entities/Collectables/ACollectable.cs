using nl.SWEG.RPGWizardry.Player.Inventory;
using System;
using System.Linq;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Entities.Collectables
{
    /// <summary>
    /// Base Class for a Collectable Object
    /// </summary>
    public abstract class ACollectable : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Tags for Collision
        /// </summary>
        [SerializeField]
        [Tooltip("Tags for Collision")]
        private string[] targetTags;
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
            if (targetTags.Contains(collision.gameObject.tag))
            {
                PlayerInventory inv = collision.gameObject.GetComponent<PlayerInventory>();
                if (inv == null)
                    throw new InvalidOperationException("Target has no Inventory");
                if (OnCollect(inv))
                    Destroy(gameObject);
            }
        }
        #endregion
    }
}