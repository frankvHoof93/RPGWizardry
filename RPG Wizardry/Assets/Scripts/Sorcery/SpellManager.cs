using UnityEngine;
using System.Collections;
using nl.SWEG.RPGWizardry.Player.Inventory;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.Sorcery
{
    public class SpellManager : MonoBehaviour
    {
        [SerializeField]
        private SpellPage selectedSpell;
        private PlayerInventory inventory;
        [SerializeField]
        private Text title;
        [SerializeField]
        private Button button;
        // Use this for initialization
        void Start()
        {
            title.text = selectedSpell.SpellTitle;
        }

        public void UnlockSpell()
        {
            inventory.UnlockSpell(selectedSpell);
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

