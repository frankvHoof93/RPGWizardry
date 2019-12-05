using nl.SWEG.RPGWizardry.Sorcery.Spells;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.UI.GameUI
{
    public class SpellHUD : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private Text nameText;
        [SerializeField]
        private Image spellImage;
        [SerializeField]
        private Image selectionOutline;
        [SerializeField]
        private Image cooldownOverlay;

        private Coroutine cooldownRoutine;
        #endregion

        #region Methods
        public void Select()
        {
            selectionOutline.enabled = true;
        }

        public void Deselect()
        {
            selectionOutline.enabled = false;
        }

        public void SetSpell(SpellData spellData)
        {
            nameText.text = spellData.Name;
            spellImage.sprite = spellData.Sprite;
            spellImage.enabled = true;
            cooldownOverlay.sprite = spellData.CooldownSprite;
            cooldownOverlay.fillAmount = 0;
        }

        public void RunCooldown(float duration)
        {
            if (cooldownRoutine != null)
                StopCoroutine(cooldownRoutine);
            if (duration == 0)
                cooldownOverlay.fillAmount = 0;
            else
                cooldownRoutine = StartCoroutine(CooldownRoutine(duration));
        }

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
    }
}