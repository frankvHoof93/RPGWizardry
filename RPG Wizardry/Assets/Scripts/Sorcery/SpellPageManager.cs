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
        private SpellPage selectedSpell;
        /// <summary>
        /// Title of spell page
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private Button button;
        [SerializeField]
        private Image spellImage;
        [SerializeField]
        private TextMeshProUGUI description;

        #endregion
        #region Methods

        public void SetSelectedSpell(SpellPage target)
        {
            selectedSpell = target;
            OnEnable(); // Set new Info
        }

        private void OnEnable()
        {
            if (selectedSpell == null)
                return;
            title.text = selectedSpell.SpellTitle;
            spellImage.sprite = selectedSpell.Spell.Sprite;
            description.text = selectedSpell.Spell.Description;
            button.enabled = !selectedSpell.Unlocked;
        }

        /// <summary>
        /// Unlocking of spell
        /// </summary>
        public void UnlockSpell()
        {
            PlayerManager.Instance.Inventory?.UnlockSpell(selectedSpell);
            button.enabled = false;
        }
        #endregion
    }
}

