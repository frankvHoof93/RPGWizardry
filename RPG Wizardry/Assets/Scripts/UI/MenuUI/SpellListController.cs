using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Player.Combat;
using nl.SWEG.RPGWizardry.Sorcery;
using nl.SWEG.RPGWizardry.UI.GameUI;
using System;
using System.Collections.Generic;
using UnityEngine;

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

        [SerializeField]
        private Transform leftPage;

        [SerializeField]
        private Transform rightPage;

        [SerializeField]
        private GameObject prevPageButton;

        [SerializeField]
        private GameObject nextPageButton;

        /// <summary>
        /// List of spells in the book.
        /// </summary>
        private List<SpellPage> pages = new List<SpellPage>();

        private int currentPage = 1;
        private int totalPages => Mathf.Clamp(Mathf.CeilToInt(pages.Count / 16f), 1, int.MaxValue);

        [SerializeField]
        private SpellHUD[] currSpells;

        private int? equipSelection;

        /// <summary>
        /// List of Spell gameobjects
        /// </summary>
        private List<GameObject> SpellTabs;

        private void OnEnable()
        {
            UpdateSpellHUD();
            SpellTabs = new List<GameObject>();
            pages = PlayerManager.Instance.Inventory.Pages;
            for (int i = (currentPage - 1) * 16; i < pages.Count && i < currentPage * 16; i++)
            {
                SpellPage page = pages[i];
                GameObject newSpellInfo = Instantiate(spellInfo);
                newSpellInfo.GetComponent<SpellTabManager>().Populate(page, this);
                if (i >= (currentPage - 1) * 16 + 8)
                    newSpellInfo.transform.SetParent(rightPage);
                else
                    newSpellInfo.transform.SetParent(leftPage);
                SpellTabs.Add(newSpellInfo);
            }
            // Set Page-Switch Buttons
            prevPageButton.SetActive(currentPage > 1); // Activate Previous Page-Button if on Page > first
            nextPageButton.SetActive(currentPage < totalPages); // Activate Next Page-Button if on Page < last
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                DeselectEquipTarget();
        }

        internal void OnSpellClick(SpellPage page)
        {
            if (equipSelection.HasValue)
            {
                if (page.Unlocked)
                {
                    PlayerManager.Instance.CastingManager.SetSpell(page.Spell, (ushort)equipSelection.Value);
                    UpdateSpellHUD();
                    DeselectEquipTarget();
                }
            }
            else SwitchCanvas(page);
        }

        /// <summary>
        /// Switches to the other canvas.
        /// </summary>
        /// <param name="page"></param>
        private void SwitchCanvas(SpellPage page)
        {
            SpellCanvas.SetActive(true);
            SpellCanvas.GetComponent<SpellPageManager>().SetSelectedSpell(page);
            transform.gameObject.SetActive(false);
        }

        public void NextPage()
        {
            OnDisable();
            currentPage = Mathf.Clamp(++currentPage, 1, totalPages);
            OnEnable();
        }

        public void SelectEquipTarget(int index)
        {
            if (index < 0 || index >= currSpells.Length)
                throw new ArgumentOutOfRangeException("index", "Invalid Index");

            if (equipSelection.HasValue)
            {
                if (equipSelection.Value == index)
                    return; // already selected
                DeselectEquipTarget();
            }                
            equipSelection = index;
            currSpells[index].Select();
        }

        public void DeselectEquipTarget()
        {
            equipSelection = null;
            for (ushort i = 0; i < currSpells.Length; i++)
                currSpells[i].Deselect();
        }

        public void PreviousPage()
        {
            OnDisable();
            currentPage = Mathf.Clamp(--currentPage, 1, totalPages);
            OnEnable();
        }

        private void OnDisable()
        {
            foreach(GameObject o in SpellTabs)
                Destroy(o);
        }

        private void UpdateSpellHUD()
        {
            CastingManager mgr = PlayerManager.Instance.CastingManager;
            for (ushort i = 0; i < currSpells.Length; i++)
                currSpells[i].SetSpell(mgr.GetSpell(i));
            if (equipSelection.HasValue)
                currSpells[equipSelection.Value].Select();
        }
    }

}