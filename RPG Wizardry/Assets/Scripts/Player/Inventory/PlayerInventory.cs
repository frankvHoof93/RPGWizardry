using nl.SWEG.RPGWizardry.Sorcery;
using nl.SWEG.RPGWizardry.Sorcery.Spells;
using nl.SWEG.RPGWizardry.Utils.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Player.Inventory
{
    public class PlayerInventory : MonoBehaviour, IStorable, IJSON<PlayerInventory>
    {
        #region InnerTypes
        /// <summary>
        /// Delegate for Changes to Inventory
        /// </summary>
        /// <param name="newAmount">Amount after Change</param>
        /// <param name="change">Change that was applied</param>
        public delegate void OnInventoryChange(uint newAmount, int change);
        #endregion

        #region Variables
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

        #region Events
        /// <summary>
        /// Event called when Dust-Amount changes
        /// </summary>
        private event OnInventoryChange dustChangeEvent;
        /// <summary>
        /// Event called when Gold-Amount changes
        /// </summary>
        private event OnInventoryChange goldChangeEvent;
        /// <summary>
        /// Event called when Page-Amount changes
        /// </summary>
        private event OnInventoryChange pageChangeEvent;
        #endregion
        #endregion
        #endregion

        #region Methods
        #region Public
        #region EventListeners
        /// <summary>
        /// Adds Listener to Dust-Event
        /// </summary>
        /// <param name="listener">Listener to Add</param>
        public void AddDustListener(OnInventoryChange listener)
        {
            dustChangeEvent += listener;
            // Set Initial Value
            listener.Invoke(Dust, 0);
        }
        /// <summary>
        /// Removes Listener from Dust-Event
        /// </summary>
        /// <param name="listener">Listener to Remove</param>
        public void RemoveDustListener(OnInventoryChange listener)
        {
            dustChangeEvent -= listener;
        }
        /// <summary>
        /// Adds Listener to Gold-Event
        /// </summary>
        /// <param name="listener">Listener to Add</param>
        public void AddGoldListener(OnInventoryChange listener)
        {
            goldChangeEvent += listener;
            // Set Initial Value
            listener.Invoke(Gold, 0);
        }
        /// <summary>
        /// Removes Listener from Gold-Event
        /// </summary>
        /// <param name="listener">Listener to Remove</param>
        public void RemoveGoldListener(OnInventoryChange listener)
        {
            goldChangeEvent -= listener;
        }
        #endregion

        #region Storage
        /// <summary>
        /// Loads Inventory from File
        /// </summary>
        /// <param name="path">Path to load from</param>
        public void Load(string path)
        {
            string json = File.ReadAllText(path);
            PlayerInventory toLoad = FromJSON(json);
            // TODO: Load variables
            // Called with 0 to not perform animation
            dustChangeEvent?.Invoke(Dust, 0);
            goldChangeEvent?.Invoke(Gold, 0);

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

        #region Getters
        public bool HasSpell(SpellData spell)
        {
            return Pages.Any(p => ReferenceEquals(spell, p.Spell));
        }
        #endregion
        #endregion

        #region Internal
        #region Add
        /// <summary>
        /// Adds Page to Inventory, if it does not exist in Inventory yet
        /// </summary>
        /// <param name="page">Page to add</param>
        internal bool AddPage(SpellPage page)
        {
            if (page != null && !pages.Any(p => ReferenceEquals(p.Spell, page.Spell)))
            {
                pages.Add(page);
                return true;
            }
            else return false;
        }
        /// <summary>
        /// Adds Dust to Inventory
        /// </summary>
        /// <param name="amount">Amount of Dust to add (>= 0)</param>
        internal void AddDust(uint amount)
        {
            Dust += amount;
            dustChangeEvent.Invoke(Dust, (int)amount);
        }
        /// <summary>
        /// Adds Gold to Inventory
        /// </summary>
        /// <param name="amount">Amount of Gold to add (>= 0)</param>
        internal void AddGold(uint amount)
        {
            Gold += amount;
            goldChangeEvent.Invoke(Gold, (int)amount);
        }
        #endregion
        #endregion
        #endregion
    }
}
