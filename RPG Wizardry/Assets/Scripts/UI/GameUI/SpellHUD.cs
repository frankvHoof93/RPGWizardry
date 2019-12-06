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
        /// <summary>
        /// Text-Object for Spell-Name
        /// </summary>
        [SerializeField]
        [Tooltip("Text-Object for Spell-Name")]
        private Text nameText;
        /// <summary>
        /// Image-Object for Spell
        /// </summary>
        [SerializeField]
        [Tooltip("Image-Object for Spell")]
        private Image spellImage;
        /// <summary>
        /// Image-Object for Selection-Outline
        /// </summary>
        [SerializeField]
        [Tooltip("Image-Object for Selection-Outline")]
        private Image selectionOutline;
        /// <summary>
        /// Image-Object for Spell-Cooldown
        /// </summary>
        [SerializeField]
        [Tooltip("Image-Object for Spell-Cooldown")]
        private Image cooldownOverlay;
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
            selectionOutline.enabled = true;
        }
        /// <summary>
        /// Deselects this spell (Disables Selection-Outline)
        /// </summary>
        public void Deselect()
        {
            selectionOutline.enabled = false;
        }
        /// <summary>
        /// Sets Spell-Data to UI-Elements
        /// </summary>
        /// <param name="spellData">Data for Spell to set</param>
        public void SetSpell(SpellData spellData)
        {
            nameText.text = spellData.Name;
            spellImage.sprite = spellData.Sprite;
            spellImage.enabled = true;
            cooldownOverlay.sprite = spellData.CooldownSprite;
            cooldownOverlay.fillAmount = 0;
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