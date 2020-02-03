using nl.SWEG.Willow.Sorcery.Spells;
using System;
using UnityEngine;

namespace nl.SWEG.Willow.Sorcery
{
    /// <summary>
    /// A SpellPage is an item which holds a Spell
    /// <para>
    /// The Page also contains data related to unlocking the Spell
    /// </para>
    /// </summary>
    [Serializable]
    public class SpellPage
    {
        #region Variables
        #region Public
        /// <summary>
        /// Data for Spell
        /// </summary>
        public SpellData Spell => spell;
        /// <summary>
        /// Name of Spell
        /// </summary>
        public string SpellTitle => spell.Name;
        /// <summary>
        /// Description of Spell
        /// </summary>
        public string SpellDescription => spell.Description;
        /// <summary>
        /// Whether the Spell on this Page has been Unlocked
        /// </summary>
        public bool Unlocked { get; private set; }
        /// <summary>
        /// Dust-Cost for unlocking Spell  
        /// </summary>
        public uint DustCost => spell.SpellCost;
        #endregion

        #region Editor
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Data for Spell
        /// </summary>
        [SerializeField]
        [Tooltip("Data for Spell")]
        private SpellData spell;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Constructor for a SpellPage
        /// </summary>
        /// <param name="spell">Spell on Page</param>
        /// <param name="unlocked">Whether the Spell has been unlocked (defaults to False)</param>
        public SpellPage(SpellData spell, bool unlocked = false)
        {
            this.spell = spell;
            Unlocked = unlocked;
        }

        /// <summary>
        /// Unlocks Spell on this Page (after crafting)
        /// </summary>
        internal void UnlockSpell()
        {
            Unlocked = true;
        }   
        #endregion
    }
}