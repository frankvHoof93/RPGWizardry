using nl.SWEG.RPGWizardry.Sorcery;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.UI.MenuUI
{
    public class SpellTabManager : MonoBehaviour
    {
        /// <summary>
        /// Target spell page for information and UI navigation
        /// </summary>
        private SpellPage page;

        /// <summary>
        /// Controller for Spell list UI interaction
        /// </summary>
        private SpellListController spellListController;

        /// <summary>
        /// Spell element Image
        /// </summary>
        [Tooltip("Spell element image")]
        [SerializeField]
        private Image element;

        /// <summary>
        /// List of sprite elements
        /// </summary>
        [Tooltip("list of element sprites")]
        [SerializeField]
        private List<Sprite> elementSprites;

        /// <summary>
        /// check image if unlocked.
        /// </summary>
        [Tooltip("Spell unlocked image")]
        [SerializeField]
        private Image check;

        /// <summary>
        /// List of check sprites
        /// </summary>
        [Tooltip("List of spell unlock status sprites")]
        [SerializeField]
        private List<Sprite> checkSprites;

        /// <summary>
        /// Spell title
        /// </summary>
        [Tooltip("Spell title")]
        [SerializeField]
        private TextMeshProUGUI title;



        /// <summary>
        /// Used to update the Spelltab with the appropriate information.
        /// </summary>
        /// <param name="page">The spell page which this spell tab represents</param>
        /// <param name="spellListController">Spell list controller used to navigate through the UI.</param>
        internal void Populate(SpellPage page, SpellListController spellListController)
        {
            this.page = page;
            element.sprite = elementSprites[(int)this.page.Spell.Element];
            if(page.Unlocked)
            {
                check.sprite = checkSprites[1];
            }
            else
            {
                check.sprite = checkSprites[0];
            }
            title.text = this.page.Spell.Name;
            this.spellListController = spellListController;
        }

        public void OnClick()
        {
            spellListController.OnSpellClick(page);
        }
    }

}
