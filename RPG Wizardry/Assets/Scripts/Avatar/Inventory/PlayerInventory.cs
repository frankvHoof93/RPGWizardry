using nl.SWEG.RPGWizardry.Sorcery;
using nl.SWEG.RPGWizardry.Utils.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Avatar.Inventory
{
    public class PlayerInventory : MonoBehaviour, IStorable, IJSON<PlayerInventory>
    {
        #region Methods
        #region Public
        /// <summary>
        /// Amount of Dust in Inventory
        /// </summary>
        public uint Dust { get; private set; }
        /// <summary>
        /// Amount of Gold in Inventory
        /// </summary>
        public uint Gold { get; private set; }
        /// <summary>
        /// Pages in Inventory
        /// </summary>
        public List<SpellPage> Pages { get { return new List<SpellPage>(); } }
        #endregion

        #region Private
        /// <summary>
        /// Pages in Inventory
        /// </summary>
        private readonly List<SpellPage> pages = new List<SpellPage>();
        #endregion
        #endregion

        #region Methods
        #region Public
        #region Storage
        /// <summary>
        /// Loads Inventory from File
        /// </summary>
        /// <param name="path"></param>
        public void Load(string path)
        {
            string json = File.ReadAllText(path);
            PlayerInventory toLoad = FromJSON(json);
            // TODO: Load variables
        }
        /// <summary>
        /// Saves Inventory to File
        /// </summary>
        public void Save(string path)
        {
            File.WriteAllText(path, ToJSON());
        }
        /// <summary>
        /// Creates JSON-representation for this Object
        /// </summary>
        /// <returns>JSON-String for this Inventory</returns>
        public string ToJSON()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Loads Inventory-Values from JSON
        /// </summary>
        /// <returns>JSON-string to load from</returns>
        public PlayerInventory FromJSON(string json)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Add
        /// <summary>
        /// Adds Page to Inventory, if it does not exist in Inventory yet
        /// </summary>
        /// <param name="page">Page to add</param>
        public bool AddPage(SpellPage page)
        {
            if (page != null && !pages.Contains(page))
            {
                pages.Add(page);
                return true;
            }
            else return false;
        }
        /// <summary>
        /// Adds Dust to Inventory
        /// </summary>
        /// <param name="amount">Amount of Dust to add (> 0)</param>
        public void AddDust(uint amount)
        {
            Dust += amount;
        }
        /// <summary>
        /// Adds Gold to Inventory
        /// </summary>
        /// <param name="amount">Amount of Gold to add (> 0)</param>
        public void AddGold(uint amount)
        {
            Gold += amount;
        }
        #endregion
        #region Unlock
        /// <summary>
        /// Unlocks a Page in the Inventory
        /// </summary>
        /// <param name="page">Page to unlock</param>
        /// <returns></returns>
        public bool UnlockSpell(SpellPage page)
        {
             page.UnlockSpell();
            return page.Unlocked;
        }

        public void ConsumeDust(SpellPage page)
        {
            if(Dust >= page.GetDustCost())
            {
                Dust -= page.GetDustCost();
            }
        }
        #endregion
        #endregion
        #endregion
    }
}
