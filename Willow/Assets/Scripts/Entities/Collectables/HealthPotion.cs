using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.Player.Inventory;
using UnityEngine;

namespace nl.SWEG.Willow.Entities.Collectables
{
    /// <summary>
    /// Health-Potion in GameWorld
    /// </summary>
    public class HealthPotion : ACollectable
    {
        #region Variables
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Amount of Health to add from this Potion
        /// </summary>
        [SerializeField]
        [Tooltip("Amount of Health to add from this Potion")]
        private ushort healAmount;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Methods
        /// <summary>
        /// Collects HealthPotion (if not a full health yet)
        /// </summary>
        /// <param name="target">Inventory for Player</param>
        /// <returns>True if Player is not at full health (potion collected)</returns>
        protected override bool OnCollect(PlayerInventory target)
        {
            return target.GetComponent<PlayerManager>().Heal(healAmount);
        }
        #endregion
    }
}