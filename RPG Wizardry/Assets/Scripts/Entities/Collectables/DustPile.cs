using nl.SWEG.RPGWizardry.Avatar.Inventory;
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
        private int Amount = 0;
        #endregion

        #region Methods
        /// <summary>
        /// Adds Dust to Inventory, then Destroys this Object
        /// </summary>
        /// <param name="target">Inventory to Add to</param>
        protected override void OnCollect(PlayerInventory target)
        {
            target.AddDust(Amount);
            Destroy(gameObject);
        }
        #endregion
    }
}