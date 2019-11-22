using nl.SWEG.RPGWizardry.Avatar;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.UI.GameUI
{
    public class HealthUI : MonoBehaviour
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

        #region Methods
        #region Unity
        /// <summary>
        /// Registers this UI to the Health-Event
        /// </summary>
        private void OnEnable()
        {
            if (AvatarManager.Exists)
                AvatarManager.Instance.AddHealthChangeListener(UpdateHealthBar);
        }
        /// <summary>
        /// Unregisters this UI from the Health-Event
        /// </summary>
        private void OnDisable()
        {
            if (AvatarManager.Exists)
                AvatarManager.Instance.RemoveHealthChangeListener(UpdateHealthBar);
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
            // TODO: Change-Popup/Effect?
            healthText.text = newHealth + "/" + maxHealth;
            float healthPercentage = (float)newHealth / (float)maxHealth;
            healthFillBar.fillAmount = healthPercentage;
            if (healthPercentage < 0.25f)
                healthFillBar.color = colors.lowHealth;
            else if (healthPercentage < 0.67f)
                healthFillBar.color = colors.mediumHealth;
            else
                healthFillBar.color = colors.lowHealth;
        }
        #endregion
        #endregion
    }
}