using nl.SWEG.Willow.Sorcery;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.Willow.UI.Spells
{
    /// <summary>
    /// UI for Spell in SpellList
    /// </summary>
    public class SpellTab : MonoBehaviour
    {
        #region Variables
        #region Editor
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Spell Title Text
        /// </summary>
        [Tooltip("Spell Title Text")]
        [SerializeField]
        private TextMeshProUGUI title;
        /// <summary>
        /// Element Image
        /// </summary>
        [Tooltip("Element Image")]
        [SerializeField]
        private Image element;
        /// <summary>
        /// Checkbox-Image for Unlock-Status
        /// </summary>
        [Tooltip("Checkbox-Image for Unlock-Status")]
        [SerializeField]
        private Image check;
        /// <summary>
        /// List of Sprites for Elements
        /// </summary>
        [Tooltip("List of Sprites for Elements")]
        [SerializeField]
        private List<Sprite> elementSprites;
        /// <summary>
        /// List of Sprites for Unlock-Status
        /// </summary>
        [Tooltip("List of Sprites for Unlock-Status")]
        [SerializeField]
        private List<Sprite> checkSprites;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Private
        /// <summary>
        /// Target SpellPage for information and UI navigation
        /// </summary>
        private SpellPage page;
        /// <summary>
        /// Controller for Spell list UI interaction
        /// </summary>
        private SpellListController spellListController;
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Updates the Spelltab with information from SpellPage
        /// </summary>
        /// <param name="page">The spell page which this spell tab represents</param>
        /// <param name="spellListController">Spell list controller used to navigate through the UI.</param>
        public void Populate(SpellPage page)
        {
            this.page = page;
            element.sprite = elementSprites[(int)page.Spell.Element];
            check.sprite = checkSprites[page.Unlocked ? 1 : 0];
            title.text = page.Spell.Name;
        }

        /// <summary>
        /// Sets controller (for OnClick-Handling)
        /// </summary>
        /// <param name="controller">Controller to set</param>
        public void SetController(SpellListController controller)
        {
            spellListController = controller;
        }

        /// <summary>
        /// Handles OnClick for opening the Details-Page for this Spell
        /// </summary>
        public void OnClick()
        {
            spellListController?.OnSpellClick(page);
        }
        #endregion
    }
}