using nl.SWEG.RPGWizardry.Player.Inventory;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Entities.Collectables
{
    public class GoldPile : ACollectable
    {
        #region Variables
        /// <summary>
        /// Amount of Gold in Pile
        /// </summary>
        [SerializeField]
        [Tooltip("Amount of Gold in Pile")]
        private uint Amount = 0;
        #endregion

        #region Methods
        /// <summary>
        /// Sets amount of Gold in Pile
        /// </summary>
        /// <param name="amount">Amount to Set</param>
        internal void SetAmount(uint amount)
        {
            Amount = amount;
        }

        /// <summary>
        /// Adds Dust to Inventory, then Destroys this Object
        /// </summary>
        /// <param name="target">Inventory to Add to</param>
        protected override bool OnCollect(PlayerInventory target)
        {
            target.AddGold(Amount);
            return true;
        }
        #endregion
    }
}