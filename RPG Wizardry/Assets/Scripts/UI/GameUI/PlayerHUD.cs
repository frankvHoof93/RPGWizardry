﻿using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Sorcery.Spells;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.UI.GameUI
{
    public class PlayerHUD : MonoBehaviour
    {
        #region Variables
        #region Health
        [Header("Health")]
        /// <summary>
        /// Fill-UI for HealthBar
        /// </summary>
        [SerializeField]
        [Tooltip("Fill-UI for HealthBar")]
        private Image healthFillBar;
        /// <summary>
        /// DEBUG Text-UI for HealthBar
        /// </summary>
        [SerializeField]
        [Tooltip("Text-UI for HealthBar")]
        private Text healthText;
        #endregion

        #region Items
        [Header("Items")]
        /// <summary>
        /// Text-UI for Dust-Amount
        /// </summary>
        [SerializeField]
        [Tooltip("Text-UI for Dust-Amount")]
        private Text dustText;
        /// <summary>
        /// Text-UI for Gold-Amount
        /// </summary>
        [SerializeField]
        [Tooltip("Text-UI for Gold-Amount")]
        private Text goldText;
        #endregion

        #region Spells
        [Header("Spells")]
        [SerializeField]
        private SpellHUD[] spellHuds;
        #endregion
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Registers this UI to Events
        /// </summary>
        private void OnEnable()
        {
            if (PlayerManager.Exists)
            {
                PlayerManager player = PlayerManager.Instance;
                player.AddHealthChangeListener(UpdateHealthBar);
                player.Inventory?.AddDustListener(UpdateDustAmount);
                player.Inventory?.AddGoldListener(UpdateGoldAmount);
                player.CastingManager?.AddSelectionListener(UpdateSpellSelection);
                player.CastingManager?.AddSpellChangeListener(UpdateSpellUI);
                player.CastingManager?.AddCastListener(UpdateSpellCooldown);
            }
        }
        /// <summary>
        /// Unregisters this UI from Events
        /// </summary>
        private void OnDisable()
        {
            if (PlayerManager.Exists)
            {
                PlayerManager player = PlayerManager.Instance;
                player.RemoveHealthChangeListener(UpdateHealthBar);
                player.Inventory?.RemoveDustListener(UpdateDustAmount);
                player.Inventory?.RemoveGoldListener(UpdateGoldAmount);
                player.CastingManager?.RemoveSelectionListener(UpdateSpellSelection);
                player.CastingManager?.RemoveSpellChangeListener(UpdateSpellUI);
                player.CastingManager?.RemoveCastListener(UpdateSpellCooldown);
            }
        }
        #endregion

        #region Private
        #region Health
        /// <summary>
        /// Updates HealthBar
        /// </summary>
        /// <param name="newHealth">New Value for Health</param>
        /// <param name="maxHealth">Max Value for Health</param>
        /// <param name="change">Change in Value from previous</param>
        private void UpdateHealthBar(ushort newHealth, ushort maxHealth, short change)
        {
            healthText.text = newHealth + "/" + maxHealth;
            float healthPercentage = (float)newHealth / (float)maxHealth;
            healthFillBar.fillAmount = healthPercentage;
            if (change != 0)
            {
                // TODO: Change-Popup/Effect?
            }
        }
        #endregion

        #region Inventory
        /// <summary>
        /// Updates Dust-Amount
        /// </summary>
        /// <param name="newAmount">New amount for Dust</param>
        /// <param name="change">Change in amount</param>
        private void UpdateDustAmount(uint newAmount, int change)
        {
            dustText.text = newAmount.ToString();
            if (change != 0)
            {
                // TODO: Change-Popup/Effect?
            }
        }
        /// <summary>
        /// Updates Gold-Amount
        /// </summary>
        /// <param name="newAmount">New amount for Gold</param>
        /// <param name="change">Change in amount</param>
        private void UpdateGoldAmount(uint newAmount, int change)
        {
            goldText.text = newAmount.ToString();
            if (change != 0)
            {
                // TODO: Change-Popup/Effect?
            }
        }
        #endregion

        #region Spells
        private void UpdateSpellSelection(ushort newSelection)
        {
            for (ushort i = 0; i < spellHuds.Length; i++)
            {
                if (i == newSelection)
                    spellHuds[i].Select();
                else
                    spellHuds[i].Deselect();
            }
        }

        private void UpdateSpellUI(ushort index, SpellData spellData)
        {
            spellHuds[index].SetSpell(spellData);
            UpdateSpellCooldown(index, 0); // Set cooldown to 0 after switching
        }

        private void UpdateSpellCooldown(ushort index, float cooldown)
        {
            spellHuds[index].RunCooldown(cooldown);
        }
        #endregion
        #endregion
        #endregion
    }
}