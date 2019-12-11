using nl.SWEG.RPGWizardry.Player.PlayerInput;
using nl.SWEG.RPGWizardry.Sorcery.Spells;
using nl.SWEG.RPGWizardry.Utils.Functions;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Player.Combat
{
    [RequireComponent(typeof(InputState))]
    public class CastingManager : MonoBehaviour
    {
        #region InnerTypes
        /// <summary>
        /// Delegate for Event when Casting a Spell
        /// </summary>
        /// <param name="index">Index for Selected Spell</param>
        /// <param name="cooldown">Cooldown after Casting</param>
        public delegate void OnCast(ushort index, float cooldown);
        /// <summary>
        /// Delegate for Event when setting a new Spell
        /// </summary>
        /// <param name="index">Index Spell is set to</param>
        /// <param name="newSpell">Spell that is set</param>
        public delegate void OnSpellChange(ushort index, SpellData newSpell);
        /// <summary>
        /// Delegate for Event when selecting a Spell
        /// </summary>
        /// <param name="newIndex">Index for selected Spell</param>
        public delegate void OnSelectionChange(ushort newIndex);
        #endregion

        #region Variables
        #region Constants
        /// <summary>
        /// Amount of slots available for Spells
        /// </summary>
        public const ushort SelectableSpellAmount = 4;
        #endregion

        #region Public
        /// <summary>
        /// DEBUG Prototype projectile; fill this with selected spell later
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
        /// Spells available for Casting (Currently Selected Spells)
        /// </summary>
        private readonly SpellData[] selectedSpells = new SpellData[SelectableSpellAmount];
        /// <summary>
        /// Cooldowns for Spells
        /// </summary>
        private readonly float[] spellCooldown = new float[SelectableSpellAmount];
        /// <summary>
        /// Index for currently selected Spell (in selectedSpells)
        /// </summary>
        private ushort selectedSpellIndex = 0;
        /// <summary>
        /// Currently running Coroutine for Casting-Animation
        /// </summary>
        private Coroutine runningRoutine;
        /// <summary>
        /// Event fired when Casting a Spell
        /// </summary>
        private event OnCast castEvent;
        /// <summary>
        /// Event fired when Spell-Selection changes
        /// </summary>
        private event OnSelectionChange selectionEvent;
        /// <summary>
        /// Event fired when Spell Changes
        /// </summary>
        private event OnSpellChange spellChangeEvent;
        #endregion
        #endregion

        #region Methods
        #region Public
        #region EventListeners
        /// <summary>
        /// Adds Listener to Cast-Event
        /// </summary>
        /// <param name="listener">Listener to Add</param>
        public void AddCastListener(OnCast listener)
        {
            castEvent += listener;
        }
        /// <summary>
        /// Removes Listener from Cast-Event
        /// </summary>
        /// <param name="listener">Listener to Remove</param>
        public void RemoveCastListener(OnCast listener)
        {
            castEvent -= listener;
        }
        /// <summary>
        /// Adds Listener to Selection-Event
        /// </summary>
        /// <param name="listener">Listener to Add</param>
        public void AddSelectionListener(OnSelectionChange listener)
        {
            selectionEvent += listener;
            // Set initial value
            listener.Invoke(selectedSpellIndex);
        }
        /// <summary>
        /// Removes Listener from Selection-Event
        /// </summary>
        /// <param name="listener">Listener to Remove</param>
        public void RemoveSelectionListener(OnSelectionChange listener)
        {
            selectionEvent -= listener;
        }
        /// <summary>
        /// Adds Listener to SpellChange-Event
        /// </summary>
        /// <param name="listener">Listener to Add</param>
        public void AddSpellChangeListener(OnSpellChange listener)
        {
            spellChangeEvent += listener;
        }
        /// <summary>
        /// Removes Listener from SpellChange-Event
        /// </summary>
        /// <param name="listener">Listener to Remove</param>
        public void RemoveSpellChangeListener(OnSpellChange listener)
        {
            spellChangeEvent -= listener;
        }
        #endregion

        /// <summary>
        /// Selects next available Spell in SelectedSpells
        /// </summary>
        public void SelectNextSpell()
        {
            ushort newIndex = (ushort)MathFunctions.Wrap(selectedSpellIndex + 1, 0, SelectableSpellAmount);
            while (selectedSpells[newIndex] == null) // No Spell in Slot
                newIndex = (ushort)MathFunctions.Wrap(newIndex + 1, 0, SelectableSpellAmount); // Try next slot
            SelectSpell(newIndex);
        }
        /// <summary>
        /// Selects previous available Spell in SelectedSpells
        /// </summary>
        public void SelectPreviousSpell()
        {
            ushort newIndex = (ushort)MathFunctions.Wrap(selectedSpellIndex - 1, 0, SelectableSpellAmount);
            while (selectedSpells[newIndex] == null) // No Spell in Slot
                newIndex = (ushort)MathFunctions.Wrap(newIndex - 1, 0, SelectableSpellAmount); // Try next slot
            SelectSpell(newIndex);
        }
        /// <summary>
        /// Selects Spell by Index (if not null)
        /// </summary>
        /// <param name="index">Index to Select</param>
        public void SelectSpell(ushort index)
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
        internal void SetSpell(SpellData spell, ushort index)
        {
            selectedSpells[index] = spell;
            spellCooldown[index] = 0;
            spellChangeEvent?.Invoke(index, spell);
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
            //If the player is allowed to shoot
            if (!GameManager.Instance.Locked)
            {
                if (runningRoutine != null)
                    StopCoroutine(runningRoutine);
                SpellData spell = selectedSpells[selectedSpellIndex];
                // Spawn Spell
                spell.SpawnSpell(spawnLocation.position, spawnLocation.up, targetingMask);
                // Run Event
                castEvent?.Invoke(selectedSpellIndex, spell.Cooldown);
                // Set animation
                bookAnimator.SetBool("Cast", true);
                runningRoutine = StartCoroutine(CoroutineMethods.RunDelayed(() => { bookAnimator.SetBool("Cast", false); }, 0.1f));
                // Set cooldown
                spellCooldown[selectedSpellIndex] = spell.Cooldown;
            }
        }
        #endregion
        #endregion
    }
}