using System;
using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.Sorcery.Spells;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.Willow.UI.Game
{
    /// <summary>
    /// Displays currently selected Spells and their Cooldown
    /// </summary>
    public class SpellHUD : MonoBehaviour
    {
        #region Variables
        #region Editor
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Image-Object for Spell
        /// </summary>
        [Header("UI")]
        [SerializeField]
        [Tooltip("Image-Object for Spell")]
        private Image spellImage;
        /// <summary>
        /// Image-Object for Spell-Cooldown
        /// </summary>
        [SerializeField]
        [Tooltip("Image-Object for Spell-Cooldown")]
        private Image cooldownOverlay;
        /// <summary>
        /// Image-Object for Selection-Outline (KeyBoard)
        /// </summary>
        [Header("Controls")]
        [SerializeField]
        [Tooltip("Image-Object for Selection-Outline (KeyBoard)")]
        private Image selectionOutlineKeyBoard;
        /// <summary>
        /// TODO: Panel which shows Controller-Button
        /// </summary>
        [SerializeField]
        [Tooltip("Panel which shows Button (Controller)")]
        private GameObject buttonPanelController;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Private
        /// <summary>
        /// Coroutine used for Cooldown. Stored so it can be stopped and reset if necessary
        /// </summary>
        private Coroutine cooldownRoutine;
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Selects this Spell (Enables Selection-Outline)
        /// </summary>
        public void Select()
        {
            selectionOutlineKeyBoard.enabled = true;
        }

        /// <summary>
        /// Deselects this spell (Disables Selection-Outline)
        /// </summary>
        public void Deselect()
        {
            selectionOutlineKeyBoard.enabled = false;
        }

        /// <summary>
        /// Sets Spell-Data to UI-Elements
        /// </summary>
        /// <param name="spellData">Data for Spell to set</param>
        public void SetSpell(SpellData spellData)
        {
            if (spellData != null)
            {
                spellImage.sprite = spellData.Sprite;
                cooldownOverlay.sprite = spellData.CooldownSprite;
                spellImage.enabled = true;
                cooldownOverlay.enabled = true;
                cooldownOverlay.fillAmount = 0;
            }
            else
            {
                spellImage.sprite = null;
                cooldownOverlay.sprite = null;
                spellImage.enabled = false;
                cooldownOverlay.enabled = false;
            }
        }

        /// <summary>
        /// Runs UI-Cooldown on Spell
        /// </summary>
        /// <param name="duration">Duration for Cooldown</param>
        public void RunCooldown(float duration)
        {
            if (cooldownRoutine != null)
                StopCoroutine(cooldownRoutine);
            if (Math.Abs(duration) < float.Epsilon)
                cooldownOverlay.fillAmount = 0;
            else
                cooldownRoutine = StartCoroutine(CooldownRoutine(duration));
        }
        #endregion

        #region Private
        /// <summary>
        /// Coroutine for Cooldown
        /// </summary>
        /// <param name="duration">Duration for Cooldown</param>
        private IEnumerator CooldownRoutine(float duration)
        {
            cooldownOverlay.fillAmount = 1;
            float current = 0;
            while (current < duration)
            {
                yield return null;
                if (!GameManager.Instance.Paused) // Only update if not paused
                    current = Mathf.Clamp(current + Time.deltaTime, 0, duration);
                cooldownOverlay.fillAmount = 1 - (current / duration);
            }
            yield return null;
            cooldownOverlay.fillAmount = 0;            
        }
        #endregion
        #endregion
    }
}