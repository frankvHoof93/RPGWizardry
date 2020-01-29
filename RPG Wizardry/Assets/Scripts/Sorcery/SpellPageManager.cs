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
        /// Amount of Symbols Displayed in Rect
        /// </summary>
        private const int SymbolCount = 9;

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
        /// Element text box
        /// </summary>
        [SerializeField]
        [Tooltip("Spell element")]
        private TextMeshProUGUI element;
        /// <summary>
        /// Damage text box
        /// </summary>
        [SerializeField]
        [Tooltip("Spell damage")]
        private TextMeshProUGUI damage;
        /// <summary>
        /// Cooldown text box
        /// </summary>
        [SerializeField]
        [Tooltip("Spell cooldown")]
        private TextMeshProUGUI cooldown;
        /// <summary>
        /// TextBox for Description
        /// </summary>
        [SerializeField]
        [Tooltip("TextBox for Description")]
        private TextMeshProUGUI description;
        /// <summary>
        /// Prefab for TextBox displaying a Symbol inside the Graphic
        /// </summary>
        [SerializeField]
        [Tooltip("Prefab for TextBox displaying a Symbol inside the Graphic")]
        private GameObject symbolPrefab;
        /// <summary>
        /// Parent for TextBox displaying a Symbol inside the Graphic
        /// </summary>
        [SerializeField]
        [Tooltip("Parent for TextBox displaying a Symbol inside the Graphic")]
        private Transform symbolParent;
        #endregion

        #region Methods
        /// <summary>
        /// Sets Info for Spell to UI
        /// </summary>
        /// <param name="target">Spell to Set</param>
        public void SetSelectedSpell(SpellPage target)
        {
            selectedSpell = target;
            OnDisable(); // Delete old Info
            OnEnable(); // Set new Info
        }
        /// <summary>
        /// Unlocks current spell in Inventory
        /// </summary>
        internal void UnlockSpell()
        {
            PlayerManager.Instance.Inventory?.UnlockSpell(selectedSpell);
            PlayerManager.Instance.CastingManager?.TryEquipSpell(selectedSpell.Spell);
            button.enabled = false;
        }
        /// <summary>
        /// Sets info for Current Spell to UI
        /// </summary>
        private void OnEnable()
        {
            uint? dust = PlayerManager.Instance?.Inventory?.Dust;
            if (selectedSpell == null)
                return;
            title.text = selectedSpell.SpellTitle;
            spellImage.sprite = selectedSpell.Spell.Sprite;
            description.text = selectedSpell.Spell.Description;
            element.text = "Element: " + selectedSpell.Spell.Element;
            damage.text = "Damage: " + selectedSpell.Spell.Damage;
            cooldown.text = "Cooldown: " + selectedSpell.Spell.Cooldown +"s";
            if(selectedSpell.Unlocked)
            {
                button.enabled = false;
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Unlocked";
            }
            else if(dust >= selectedSpell.DustCost)
            {
                button.enabled = !selectedSpell.Unlocked;
                button.GetComponentInChildren<TextMeshProUGUI>().text = dust + "/ " + selectedSpell.DustCost;
            }
            else
            {
                button.enabled = false;
                button.GetComponentInChildren<TextMeshProUGUI>().text = dust + "/ " + selectedSpell.DustCost;
            }
            // Seed Random with Hash of Spell-Name (for consistency)
            System.Random r = new System.Random(selectedSpell.Spell.Name.GetHashCode());
            for (int i = 0; i < SymbolCount; i++) // Create Symbols
            {
                GameObject symbolInstance = Instantiate(symbolPrefab); // Create Instance
                symbolInstance.SetActive(true);
                symbolInstance.transform.SetParent(symbolParent); // Set to Parent 
                symbolInstance.transform.localScale = Vector3.one;
                TextMeshProUGUI symbol = symbolInstance.GetComponentInChildren<TextMeshProUGUI>(true);                
                symbol.SetText($"<sprite={r.Next(0, 6)}>"); // Set Symbol
                Vector3 pos = symbol.transform.localPosition;
                pos.y = (float)r.NextDouble() * 150f - 75f; // Set to random Y-pos between lower & upper bound of rect
                symbol.transform.localPosition = pos; // Set y-position
            }
        }
        /// <summary>
        /// Destroys Existing Symbols on Page
        /// </summary>
        private void OnDisable()
        {
            foreach (Transform child in symbolParent)
                Destroy(child.gameObject);
        }
        #endregion
    }
}

