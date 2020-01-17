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
        private Image element;

        [SerializeField]
        private List<Sprite> checkSprites;
        [SerializeField]
        private List<Sprite> elementSprites;

        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private Image check;

        internal void Populate(SpellPage page, SpellListController spellListController)
        {
            Page = page;
            element.sprite = elementSprites[(int)Page.Spell.Element];
            if(page.Unlocked)
            {
                check.sprite = checkSprites[1];
            }
            else
            {
                check.sprite = checkSprites[0];
            }
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
