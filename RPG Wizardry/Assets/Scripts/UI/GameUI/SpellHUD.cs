using nl.SWEG.RPGWizardry.Sorcery.Spells;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.UI.GameUI
{
    public class SpellHUD : MonoBehaviour
    {
        #region Variables
        #region Editor
        [Header("UI")]
        /// <summary>
        /// Image-Object for Spell
        /// </summary>
        [SerializeField]
        [Tooltip("Image-Object for Spell")]
        private Image spellImage;
        /// <summary>
        /// Image-Object for Spell-Cooldown
        /// </summary>
        [SerializeField]
        [Tooltip("Image-Object for Spell-Cooldown")]
        private Image cooldownOverlay;
        [Header("Controls")]
        /// <summary>
        /// Image-Object for Selection-Outline (KeyBoard)
        /// </summary>
        [SerializeField]
        [Tooltip("Image-Object for Selection-Outline (KeyBoard)")]
        private Image selectionOutlineKeyBoard;
        /// <summary>
        /// Panel which shows Button (Controller)
        /// </summary>
        [SerializeField]
        [Tooltip("Panel which shows Button (Controller)")]
        private GameObject buttonPanelController;
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
            if (duration == 0)
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
            float curr = 0;
            while (curr < duration)
            {
                yield return null;
                if (!GameManager.Instance.Paused) // Only update if not paused
                    curr = Mathf.Clamp(curr + Time.deltaTime, 0, duration);
                cooldownOverlay.fillAmount = 1 - (curr / duration);
            }
            yield return null;
            cooldownOverlay.fillAmount = 0;            
        }
        #endregion
        #endregion
    }
}