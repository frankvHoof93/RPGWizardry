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

        //TODO Dustcost should be retrieved from the SpellData object
        /// <summary>
        /// The dust cost for the spell to unlock.  
        /// </summary>
        public uint DustCost { get; private set; }
        /// <summary>
        /// Data for Spell
        /// </summary>
        [SerializeField]
        [Tooltip("Data for Spell")]
        private SpellData spell;

        /// <summary>
        /// Spell title returned from the spell object.
        /// </summary>
        public string SpellTitle { get => spell.Name; }
        #endregion


        #region Methods
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