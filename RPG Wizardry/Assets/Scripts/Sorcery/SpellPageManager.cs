using nl.SWEG.RPGWizardry.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.Sorcery
{
    public class SpellPageManager : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Spell targeted for unlocking
        /// </summary>
        [SerializeField]
        [Tooltip("Spell targeted for unlocking")]
        private SpellPage selectedSpell;
        /// <summary>
        /// Title of spell page
        /// </summary>
        [SerializeField]
        [Tooltip("Title of spell page")]
        private TextMeshProUGUI title;
        /// <summary>
        /// SpellUnlocking-Button
        /// </summary>
        [SerializeField]
        [Tooltip("SpellUnlocking-Button")]
        private Button button;
        /// <summary>
        /// Image for Spell Page
        /// </summary>
        [SerializeField]
        [Tooltip("Image for Spell Page")]
        private Image spellImage;
        /// <summary>
        /// TextBox for Description
        /// </summary>
        [SerializeField]
        [Tooltip("TextBox for Description")]
        private TextMeshProUGUI description;
        #endregion

        #region Methods
        /// <summary>
        /// Sets Info for Spell to UI
        /// </summary>
        /// <param name="target">Spell to Set</param>
        public void SetSelectedSpell(SpellPage target)
        {
            selectedSpell = target;
            OnEnable(); // Set new Info
        }
        /// <summary>
        /// Unlocks current spell in Inventory
        /// </summary>
        internal void UnlockSpell()
        {
            PlayerManager.Instance.Inventory?.UnlockSpell(selectedSpell);
            button.enabled = false;
        }
        /// <summary>
        /// Sets info for Current Spell to UI
        /// </summary>
        private void OnEnable()
        {
            if (selectedSpell == null)
                return;
            title.text = selectedSpell.SpellTitle;
            spellImage.sprite = selectedSpell.Spell.Sprite;
            description.text = selectedSpell.Spell.Description;
            button.enabled = !selectedSpell.Unlocked;
        }
        #endregion
    }
}

