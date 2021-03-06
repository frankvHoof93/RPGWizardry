﻿using nl.SWEG.Willow.Player.Inventory;
using UnityEngine;

namespace nl.SWEG.Willow.Entities.Collectables
{
    /// <summary>
    /// Pile of Gold Coins in GameWorld
    /// </summary>
    public class GoldPile : ACollectable
    {
        #region Variables
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Amount of Gold in Pile
        /// </summary>
        [SerializeField]
        [Tooltip("Amount of Gold in Pile")]
        private uint amount;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Methods
        /// <summary>
        /// Sets amount of Gold in Pile
        /// </summary>
        /// <param name="toSet">Amount to Set</param>
        internal void SetAmount(uint toSet)
        {
            amount = toSet;
        }

        /// <summary>
        /// Adds Dust to Inventory
        /// </summary>
        /// <param name="target">Inventory to Add to</param>
        /// <returns>True if adding was successful</returns>
        protected override bool OnCollect(PlayerInventory target)
        {
            target.AddGold(amount);
            return true;
        }
        #endregion
    }
}