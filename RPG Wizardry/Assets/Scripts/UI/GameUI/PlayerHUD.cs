using nl.SWEG.RPGWizardry.Player;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.UI.GameUI
{
    public class PlayerHUD : MonoBehaviour
    {
        #region InnerTypes
        /// <summary>
        /// Struct used to store Colors for HealthBar
        /// </summary>
        [Serializable]
        private struct HealthColors
        {
            public Color fullHealth;
            public Color mediumHealth;
            public Color lowHealth;
        }
        #endregion

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
        /// Text-UI for HealthBar
        /// </summary>
        [SerializeField]
        [Tooltip("Text-UI for HealthBar")]
        private Text healthText;
        /// <summary>
        /// Colors for FillBar
        /// </summary>
        [SerializeField]
        [Tooltip("Colors for FillBar")]
        private HealthColors colors;
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
            }
        }
        /// <summary>
        /// Sets default values
        /// </summary>
        private void Reset()
        {
            colors = new HealthColors
            {
                fullHealth = Color.green,
                mediumHealth = Color.yellow,
                lowHealth = Color.red
            };
        }
        #endregion

        #region Private
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
            if (healthPercentage < 0.25f)
                healthFillBar.color = colors.lowHealth;
            else if (healthPercentage < 0.67f)
                healthFillBar.color = colors.mediumHealth;
            else
                healthFillBar.color = colors.fullHealth;
            if (change != 0)
            {
                // TODO: Change-Popup/Effect?
            }
        }
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
        #endregion
    }
}