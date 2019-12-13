using UnityEngine;
using System.Collections;
using nl.SWEG.RPGWizardry.Player.Inventory;
using UnityEngine.UI;
using TMPro;

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
        void Start()
        {
            title.text = selectedSpell.SpellTitle;
        }

        /// <summary>
        /// Unlocking of spell
        /// </summary>
        public void UnlockSpell()
        {
            if(inventory != null)
            {
                inventory.UnlockSpell(selectedSpell);
            }
            
        }
        // Update is called once per frame
        void Update()
        {
            if (selectedSpell.Unlocked && button.enabled)
            {
                button.enabled = false;
            }

        }
    }
}

