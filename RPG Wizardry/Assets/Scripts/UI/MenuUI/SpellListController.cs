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
        #region Variables
        #region Editor
        [Header("Spell-Details")]
        /// <summary>
        /// Spell-Info Prefab
        /// </summary>
        [SerializeField]
        [Tooltip("Spell-Info Prefab")]
        private GameObject spellInfo;
        /// <summary>
        /// Menu for Spell-Details
        /// </summary>
        [SerializeField]
        [Tooltip("Menu for Spell-Details")]
        private GameObject SpellCanvas;
        [Header("List")]
        /// <summary>
        /// Left Page for List
        /// </summary>
        [SerializeField]
        [Tooltip("Left Page for List")]
        private Transform leftPage;
        /// <summary>
        /// Right Page for List
        /// </summary>
        [SerializeField]
        [Tooltip("Right Page for List")]
        private Transform rightPage;
        /// <summary>
        /// Previous Page-Button
        /// </summary>
        [SerializeField]
        [Tooltip("Previous Page-Button")]
        private GameObject prevPageButton;
        /// <summary>
        /// Next Page-Button
        /// </summary>
        [SerializeField]
        [Tooltip("Next Page-Button")]
        private GameObject nextPageButton;
        [Header("Equipping")]
        /// <summary>
        /// UI for currently selected Spells
        /// </summary>
        [SerializeField]
        [Tooltip("UI for currently selected Spells")]
        private SpellHUD[] currSpells;
        #endregion

        #region Private
        /// <summary>
        /// List of spells in the book.
        /// </summary>
        private List<SpellPage> pages = new List<SpellPage>();
        /// <summary>
        /// Currently Displayed Page for List
        /// </summary>
        private int currentPage = 1;
        /// <summary>
        /// Total amount of Pages in List
        /// </summary>
        private int totalPages => Mathf.Clamp(Mathf.CeilToInt(pages.Count / 16f), 1, int.MaxValue);
        /// <summary>
        /// Current Selection for Equipping
        /// </summary>
        private int? equipSelection;
        /// <summary>
        /// UI-Objects in List
        /// </summary>
        private readonly List<GameObject> SpellTabs = new List<GameObject>();
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Moves to Previous Page in List
        /// </summary>
        public void PreviousPage()
        {
            OnDisable();
            currentPage = Mathf.Clamp(--currentPage, 1, totalPages);
            OnEnable();
        }
        /// <summary>
        /// Moves to Next Page in List
        /// </summary>
        public void NextPage()
        {
            OnDisable();
            currentPage = Mathf.Clamp(++currentPage, 1, totalPages);
            OnEnable();
        }
        /// <summary>
        /// Selects Target for Equipping
        /// </summary>
        /// <param name="index">Index for Target</param>
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
        /// <summary>
        /// Deselects Target for Equipping
        /// </summary>
        public void DeselectEquipTarget()
        {
            equipSelection = null;
            for (ushort i = 0; i < currSpells.Length; i++)
                currSpells[i].Deselect();
        }
        #endregion

        #region Internal
        /// <summary>
        /// Handles Clicking of Spell-Object in List (Equip/Details)
        /// </summary>
        /// <param name="page">Clicked SpellPage</param>
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
        #endregion

        #region Unity
        /// <summary>
        /// Initializes List
        /// </summary>
        private void OnEnable()
        {
            UpdateSpellHUD();
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
        /// <summary>
        /// Destroys List
        /// </summary>
        private void OnDisable()
        {
            foreach (GameObject o in SpellTabs)
                Destroy(o);
            SpellTabs.Clear();
        }
        /// <summary>
        /// Handles Right-Click for Deselecting Equip-Target
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                DeselectEquipTarget();
        }
        #endregion

        #region Private
        /// <summary>
        /// Switches to Detail-Page
        /// </summary>
        /// <param name="page">Selected SpellPage</param>
        private void SwitchCanvas(SpellPage page)
        {
            SpellCanvas.SetActive(true);
            SpellCanvas.GetComponent<SpellPageManager>().SetSelectedSpell(page);
            transform.gameObject.SetActive(false);
        }
        /// <summary>
        /// Updates UI for Equipped Spells
        /// </summary>
        private void UpdateSpellHUD()
        {
            CastingManager mgr = PlayerManager.Instance.CastingManager;
            for (ushort i = 0; i < currSpells.Length; i++)
                currSpells[i].SetSpell(mgr.GetSpell(i));
            if (equipSelection.HasValue)
                currSpells[equipSelection.Value].Select();
        }
        #endregion
        #endregion
    }
}