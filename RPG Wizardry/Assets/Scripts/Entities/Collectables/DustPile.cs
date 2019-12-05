using nl.SWEG.RPGWizardry.Player.Inventory;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Entities.Collectables
{
    public class DustPile : ACollectable
    {
        #region Variables
        /// <summary>
        /// Amount of Dust in Pile
        /// </summary>
        [SerializeField]
        [Tooltip("Amount of Dust in Pile")]
        private uint Amount = 0;
        #endregion

        #region Methods
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
            target.AddDust(Amount);
            return true;
        }
        #endregion
    }
}