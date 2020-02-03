using nl.SWEG.Willow.Player.Inventory;
using nl.SWEG.Willow.Sorcery;
using UnityEngine;

namespace nl.SWEG.Willow.Entities.Collectables
{
    /// <summary>
    /// Spell-Page in GameWorld
    /// </summary>
    public class PageObject : ACollectable
    {
        #region Variables
        /// <summary>
        /// Sets SpellPage on this Object
        /// </summary>
        internal SpellPage Page
        {
            get { return page; }
            set { page = value; }
        }

        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// SpellPage in Object
        /// </summary>
        [SerializeField]
        [Tooltip("SpellPage in Object")]
        private SpellPage page;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Methods
        /// <summary>
        /// Adds Page to the Inventory
        /// <para>
        /// Destroys page if it was already in the Inventory
        /// </para>
        /// </summary>
        /// <param name="target">Inventory to Add to</param>
        /// <returns>True if adding was successful (Player did not yet have this spell)</returns>
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