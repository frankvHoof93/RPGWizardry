using nl.SWEG.RPGWizardry.Sorcery.Spells;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Sorcery
{
    [System.Serializable]
    public class SpellPage
    {
        #region Variables
        /// <summary>
        /// Whether the Spell on this Page has been Unlocked
        /// </summary>
        public bool Unlocked { get; private set; }
        /// <summary>
        /// Data for Spell
        /// </summary>
        public SpellData Spell => spell;
        /// <summary>
        /// Data for Spell
        /// </summary>
        [SerializeField]
        [Tooltip("Data for Spell")]
        private SpellData spell;
        #endregion

        #region Methods
        /// <summary>
        /// Constructor for a SpellPage
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="unlocked"></param>
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