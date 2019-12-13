using nl.SWEG.RPGWizardry.Player.Inventory;
using nl.SWEG.RPGWizardry.Sorcery;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Entities.Collectables
{
    public class PageObject : ACollectable
    {
        #region Variables
        internal SpellPage Page
        {
            set { page = value; }
        }

        /// <summary>
        /// SpellPage in Object
        /// </summary>
        [SerializeField]
        [Tooltip("SpellPage in Object")]
        private SpellPage page;
        #endregion

        #region Methods
        /// <summary>
        /// Adds Page to the Inventory, then Destroys this Object
        /// </summary>
        /// <param name="target">Inventory to Add to</param>
        protected override bool OnCollect(PlayerInventory target)
        {
            bool value = target.AddPage(page);
            if (!value)
                Destroy(gameObject); // Player already has this spell. Destroy gameobject
            return value; // Destroying of GameObject handled by base-class
        }
        #endregion
    }
}
