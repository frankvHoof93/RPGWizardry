using nl.SWEG.RPGWizardry.PlayerInput;
using nl.SWEG.RPGWizardry.Sorcery.Spells;
using nl.SWEG.RPGWizardry.Utils.Functions;
using nl.SWEG.RPGWizardry.Utils.Storage;
using System;
using System.Collections;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Avatar.Combat
{
    [RequireComponent(typeof(InputState))]
    public class CastingManager : MonoBehaviour
    {
        #region Variables
        #region Constants
        private const int SelectableSpellAmount = 4;
        #endregion

        #region Public
        /// <summary>
        /// Prototype projectile; fill this with selected spell later
        /// </summary>
        public SpellData CurrentSpell;
        #endregion

        #region Editor
        /// <summary>
        /// Transform of the object the projectiles need to spawn from
        /// </summary>
        [SerializeField]
        [Tooltip("Transform of the object the projectiles need to spawn from")]
        private Transform spawnLocation;
        /// <summary>
        /// LayerMask for Entities that can be hit by cast objects
        /// </summary>
        [SerializeField]
        [Tooltip("LayerMask for Entities that can be hit by cast objects")]
        private LayerMask targetingMask;
        /// <summary>
        /// Animator for Greg
        /// </summary>
        [SerializeField]
        [Tooltip("Animator for Greg")]
        private Animator bookAnimator;
        #endregion

        #region Private
        /// <summary>
        /// Inputstate for getting button states
        /// </summary>
        private InputState inputState;
        /// <summary>
        /// Cooldown state during which you cannot cast spells
        /// </summary>
        private bool cooldown;

        private readonly SpellData[] selectedSpells = new SpellData[SelectableSpellAmount];

        private int selectedSpellIndex = 0;
        #endregion
        #endregion

        #region Methods
        #region Public
        public void SelectNextSpell()
        {
            int newIndex = MathFunctions.Wrap(selectedSpellIndex + 1, 0, SelectableSpellAmount);
            while (selectedSpells[newIndex] == null) // No Spell in Slot
                newIndex = MathFunctions.Wrap(newIndex + 1, 0, SelectableSpellAmount); // Try next slot
            SelectSpell(newIndex);
        }

        public void SelectPreviousSpell()
        {
            int newIndex = MathFunctions.Wrap(selectedSpellIndex - 1, 0, SelectableSpellAmount);
            while (selectedSpells[newIndex] == null) // No Spell in Slot
                newIndex = MathFunctions.Wrap(newIndex - 1, 0, SelectableSpellAmount); // Try next slot
            SelectSpell(newIndex);
        }

        public void SelectSpell(int index)
        {
            if (index == selectedSpellIndex)
                return; // Already selected
            if (selectedSpells[index] == null)
                return; // No Spell in slot
            selectedSpellIndex = index;
        }
        #endregion

        #region Internal
        internal void SetSpell(SpellData spell, int index)
        {
            selectedSpells[index] = spell;
        }
        #endregion

        #region Unity
        /// <summary>
        /// Grabs inputstate reference for button presses
        /// </summary>
        private void Start()
        {
            inputState = GetComponent<InputState>();
        }

        /// <summary>
        /// If the button is pressed and there's no cooldown, fire the spell
        /// </summary>
        private void Update()
        {
            if (inputState.Cast1)
            {
                if (!cooldown)
                {
                    SpawnProjectile();
                }
            }
        }
        #endregion

        #region Private

        /// <summary>
        /// Spawns a projectile, then puts the casting system on cooldown based on the spell
        /// </summary>
        /// <param name="projectile">Projectile of the spell to cast; contains cooldown value</param>
        private void SpawnProjectile()
        {
            //spawn projectile at book's location
            CurrentSpell.SpawnSpell(spawnLocation.position, spawnLocation.up, targetingMask);
            //start animation
            bookAnimator.SetBool("Cast", true);
            //start cooldown
            cooldown = true;
            StartCoroutine(Cooldown(CurrentSpell.Cooldown));
        }

        /// <summary>
        /// Cooldown state during which no spells may be cast
        /// </summary>
        /// <param name="coolSeconds">Seconds the cooldown should remain active</param>
        private IEnumerator Cooldown(float coolSeconds)
        {
            yield return new WaitForSeconds(0.1f);
            //turn off animation so it only plays once
            bookAnimator.SetBool("Cast", false);
            yield return new WaitForSeconds(coolSeconds - 0.1f);
            cooldown = false;

        }
        #endregion
        #endregion
    }
}