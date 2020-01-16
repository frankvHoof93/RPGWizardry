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
        public SpellPage Page { get; private set; }

        private SpellListController spellListController;

        [SerializeField]
        private Image Element;

        [SerializeField]
        private List<Sprite> sprites;

        [SerializeField]
        private TextMeshProUGUI title;

        internal void Populate(SpellPage page, SpellListController spellListController)
        {
            Page = page;
            Element.sprite = sprites[(int)Page.Spell.Element];
            title.text = Page.Spell.Name;
            this.spellListController = spellListController;
        }

        public void OnClick()
        {
            spellListController.SwitchCanvas(Page);
        }
        

        
        // Start is called before the first frame update
    }

}
