using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Player.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.Sorcery
{
    public class SpellPageManager : MonoBehaviour
    {
        /// <summary>
        /// Spell targeted for unlocking
        /// </summary>
        [SerializeField]
        private SpellPage selectedSpell;
        /// <summary>
        /// Player inventory  which contains current spell pages;
        /// </summary>
        [SerializeField]
        private PlayerInventory inventory;
        /// <summary>
        /// Title of spell page
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private Button button;
        private void Start()
        {
            title.text = selectedSpell.SpellTitle;
        }

        private void OnEnable()
        {
            title.text = selectedSpell.SpellTitle;
        }

        /// <summary>
        /// Unlocking of spell
        /// </summary>
        public void UnlockSpell()
        {
                inventory?.UnlockSpell(selectedSpell);
            
        }
        // Update is called once per frame
        private void Update()
        {
            if (selectedSpell.Unlocked && button.enabled)
            {
                button.enabled = false;
            }

        }
    }
}

