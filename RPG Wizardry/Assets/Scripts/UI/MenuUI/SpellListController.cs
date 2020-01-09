using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Sorcery;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.UI.MenuUI
{
    public class SpellListController : MonoBehaviour
    {
        /// <summary>
        /// Spell info object
        /// </summary>
        [SerializeField]
        private GameObject spellInfo;

        /// <summary>
        /// Spell menu after clicking on a spell;
        /// </summary>
        [SerializeField]
        private GameObject SpellCanvas;

        /// <summary>
        /// The player
        /// </summary>
        [SerializeField]
        private PlayerManager player;

        /// <summary>
        /// List of spells in the book.
        /// </summary>
        private List<SpellPage> pages;

        /// <summary>
        /// List of Spell gameobjects
        /// </summary>
        private List<GameObject> SpellTabs;

        private void OnEnable()
        {
            SpellTabs = new List<GameObject>();
            if(player.Inventory.Pages != null)
            {
                pages = player.Inventory.Pages;
                foreach (SpellPage page in pages)
                {
                    GameObject newSpellInfo = Instantiate(spellInfo);
                    newSpellInfo.GetComponent<SpellTabManager>().Populate(page, this);
                    newSpellInfo.transform.SetParent(transform.Find("Book").Find("Left Page"));
                    SpellTabs.Add(newSpellInfo);

                }
            }
        }

        /// <summary>
        /// Switches to the other canvas.
        /// </summary>
        /// <param name="page"></param>
        internal void SwitchCanvas(SpellPage page)
        {
            SpellCanvas.SetActive(true);
            SpellCanvas.GetComponent<SpellPageManager>().SetSelectedSpell(page);
            transform.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            foreach(GameObject o in SpellTabs)
            {
                Destroy(o);
            }
        }
    }

}