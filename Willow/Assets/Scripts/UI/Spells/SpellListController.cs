using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.Player.Combat;
using nl.SWEG.Willow.Sorcery;
using nl.SWEG.Willow.UI.Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.Willow.UI.Spells
{
    /// <summary>
    /// Displays list of current Spells in Inventory
    /// </summary>
    public class SpellListController : MonoBehaviour
    {
        #region Variables
        #region Editor
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        #region SpellDetails
        /// <summary>
        /// Prefab for Spell-Info
        /// </summary>
        [Header("Spell-Details")]
        [SerializeField]
        [Tooltip("Prefab for Spell-Info")]
        private GameObject spellInfo;
        /// <summary>
        /// Menu for Spell-Details
        /// </summary>
        [SerializeField]
        [Tooltip("Menu for Spell-Details")]
        private SpellPageManager spellCanvas;
        #endregion

        #region List
        /// <summary>
        /// Left Page for List
        /// </summary>
        [Header("List")]
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
        #endregion

        #region Equipping
        /// <summary>
        /// UI for currently selected Spells
        /// </summary>
        [Header("Equipping")]
        [SerializeField]
        [Tooltip("UI for currently selected Spells")]
        private SpellHUD[] currentSpells;
        #endregion
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Private
        /// <summary>
        /// List of spells in Inventory
        /// </summary>
        private IReadOnlyList<SpellPage> pages;
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
        private readonly List<SpellTab> spellTabs = new List<SpellTab>();
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
            if (index < 0 || index >= currentSpells.Length)
                throw new ArgumentOutOfRangeException(nameof(index), "Invalid Index");

            if (equipSelection.HasValue)
            {
                if (equipSelection.Value == index)
                    return; // already selected
                DeselectEquipTarget();
            }
            equipSelection = index;
            currentSpells[index].Select();
        }

        /// <summary>
        /// Deselects Target for Equipping
        /// </summary>
        public void DeselectEquipTarget()
        {
            equipSelection = null;
            for (ushort i = 0; i < currentSpells.Length; i++)
                currentSpells[i].Deselect();
        }
        #endregion

        #region Internal
        /// <summary>
        /// Handles Clicking of Spell-Object in List (Equip/Details)
        /// </summary>
        /// <param name="page">Clicked SpellPage</param>
        internal void OnSpellClick(SpellPage page)
        {
            if (equipSelection.HasValue) // Check if a position has been selected for the spell
            {
                if (page.Unlocked) // Check if page has been Unlocked
                {
                    // Set Spell to Position
                    PlayerManager.Instance.CastingManager.SetSpell(page.Spell, (ushort)equipSelection.Value);
                    UpdateSpellHUD();
                    DeselectEquipTarget();
                }
            }
            else OpenDetails(page); // Open Details Page for Spell
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
                SpellTab newSpellInfo = Instantiate(spellInfo).GetComponent<SpellTab>();
                newSpellInfo.SetController(this);
                newSpellInfo.Populate(page);
                newSpellInfo.transform.SetParent(i >= (currentPage - 1) * 16 + 8 ? rightPage : leftPage);
                spellTabs.Add(newSpellInfo);
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
            foreach (SpellTab o in spellTabs)
                Destroy(o.gameObject);
            spellTabs.Clear();
        }

        /// <summary>
        /// Handles Right-Click for Deselecting Equip-Target
        /// </summary>
        private void Update()
        {
            if (equipSelection != null && Input.GetMouseButtonDown(1))
                DeselectEquipTarget();
        }
        #endregion

        #region Private
        /// <summary>
        /// Switches to Details-Page
        /// </summary>
        /// <param name="page">SpellPage to display details for</param>
        private void OpenDetails(SpellPage page)
        {
            spellCanvas.gameObject.SetActive(true);
            spellCanvas.SetSelectedSpell(page);
            transform.gameObject.SetActive(false);
        }

        /// <summary>
        /// Updates UI for Equipped Spells
        /// </summary>
        private void UpdateSpellHUD()
        {
            CastingManager mgr = PlayerManager.Instance.CastingManager;
            for (ushort i = 0; i < currentSpells.Length; i++)
                currentSpells[i].SetSpell(mgr.GetSpell(i));
            if (equipSelection.HasValue)
                currentSpells[equipSelection.Value].Select();
        }
        #endregion
        #endregion
    }
}