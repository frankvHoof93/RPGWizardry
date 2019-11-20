﻿using nl.SWEG.RPGWizardry.Avatar.Inventory;
using nl.SWEG.RPGWizardry.Sorcery;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Entities.Collectables
{
    public class PageObject : ACollectable
    {
        #region Variables
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
        protected override void OnCollect(PlayerInventory target)
        {
            target.AddPage(page);
            Destroy(gameObject);
        }
        #endregion
    }
}
