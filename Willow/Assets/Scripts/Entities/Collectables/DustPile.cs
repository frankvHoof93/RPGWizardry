using nl.SWEG.Willow.Player.Inventory;
using UnityEngine;

namespace nl.SWEG.Willow.Entities.Collectables
{
    /// <summary>
    /// Pile of Magic Dust in GameWorld
    /// </summary>
    public class DustPile : ACollectable
    {
        #region Variables
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Amount of Dust in Pile
        /// </summary>
        [SerializeField]
        [Tooltip("Amount of Dust in Pile")]
        private uint Amount = 0;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Methods
        /// <summary>
        /// Sets amount of Dust in Pile
        /// </summary>
        /// <param name="amount">Amount to Set</param>
        internal void SetAmount(uint amount)
        {
            Amount = amount;
        }

        /// <summary>
        /// Adds Dust to Inventory
        /// </summary>
        /// <param name="target">Inventory to Add to</param>
        /// <returns>True if adding was successful</returns>
        protected override bool OnCollect(PlayerInventory target)
        {
            target.AddDust(Amount);
            return true;
        }
        #endregion
    }
}