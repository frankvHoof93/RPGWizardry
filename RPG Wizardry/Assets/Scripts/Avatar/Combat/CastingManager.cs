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
        private readonly SpellData[] selectedSpells = new SpellData[SelectableSpellAmount];
        private readonly float[] spellCooldown = new float[SelectableSpellAmount];
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
        /// <summary>
        /// Sets Spell to SelectedSpells
        /// </summary>
        /// <param name="spell">Spell to set</param>
        /// <param name="index">Index to set Spell to</param>
        internal void SetSpell(SpellData spell, int index)
        {
            selectedSpells[index] = spell;
            spellCooldown[index] = 0;
        }
        #endregion

        #region Unity
        /// <summary>
        /// Grabs inputstate reference for button presses
        /// </summary>
        private void Start()
        {
            inputState = GetComponent<InputState>();
            //DEBUG (Set serialized spell to position 0 in SelectedSpells)
            SetSpell(CurrentSpell, 0);
        }

        /// <summary>
        /// Handles Input and Spell-Cooldowns
        /// </summary>
        private void Update()
        {
            for (int i = 0; i < spellCooldown.Length; i++)
                spellCooldown[i] = Mathf.Clamp(spellCooldown[i] - Time.deltaTime, 0, float.MaxValue);
            if (inputState.Cast1 && spellCooldown[selectedSpellIndex] == 0)
                CastSpell();
        }
        #endregion

        #region Private
        /// <summary>
        /// Casts currently selected Spell
        /// </summary>
        private void CastSpell()
        {
            SpellData spell = selectedSpells[selectedSpellIndex];
            // Spawn Spell
            spell.SpawnSpell(spawnLocation.position, spawnLocation.up, targetingMask);
            // Set animation
            bookAnimator.SetBool("Cast", true);
            // TODO: Check if this coroutine might need to be cancelled at some point (e.g. cast->switch spell->cast)
            StartCoroutine(CoroutineMethods.RunDelayed(() => { bookAnimator.SetBool("Cast", false); }, 0.1f));
            // Set cooldown
            spellCooldown[selectedSpellIndex] = spell.Cooldown;
        }
        #endregion
        #endregion
    }
}