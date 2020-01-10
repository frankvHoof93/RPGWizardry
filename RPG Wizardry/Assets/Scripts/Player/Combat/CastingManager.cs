using nl.SWEG.RPGWizardry.Player.PlayerInput;
using nl.SWEG.RPGWizardry.Sorcery.Spells;
using nl.SWEG.RPGWizardry.Utils.Functions;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Player.Combat
{
    [RequireComponent(typeof(PlayerManager))]
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
        public SpellData[] CurrentSpells;
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
        /// <summary>
        /// Timeout for Scrolling (timeout between Selections)
        /// </summary>
        [SerializeField]
        private float scrollTimeOut = 0.25f;
        #endregion

        #region Private
        /// <summary>
        /// Manager for Player
        /// </summary>
        private PlayerManager player;
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
        /// <summary>
        /// Current Timeout for Scrolling (Spell-Selection)
        /// </summary>
        private float currScrollTimeout = 0f;
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
            if (currScrollTimeout > 0)
                return;
            ushort newIndex = (ushort)MathFunctions.Wrap(selectedSpellIndex + 1, 0, SelectableSpellAmount);
            while (selectedSpells[newIndex] == null) // No Spell in Slot
                newIndex = (ushort)MathFunctions.Wrap(newIndex + 1, 0, SelectableSpellAmount); // Try next slot
            SelectSpell(newIndex);
            currScrollTimeout = scrollTimeOut;
        }
        /// <summary>
        /// Selects previous available Spell in SelectedSpells
        /// </summary>
        public void SelectPreviousSpell()
        {
            if (currScrollTimeout > 0)
                return;
            ushort newIndex = (ushort)MathFunctions.Wrap(selectedSpellIndex - 1, 0, SelectableSpellAmount);
            while (selectedSpells[newIndex] == null) // No Spell in Slot
                newIndex = (ushort)MathFunctions.Wrap(newIndex - 1, 0, SelectableSpellAmount); // Try next slot
            SelectSpell(newIndex);
            currScrollTimeout = scrollTimeOut;
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
            selectionEvent.Invoke(selectedSpellIndex);
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
            player = GetComponent<PlayerManager>();
            //DEBUG (Set serialized spell to position 0 in SelectedSpells)
            for (int i = 3; i >= 0; i--)
            {
                if (i >= CurrentSpells.Length)
                    continue;
                SetSpell(CurrentSpells[i], (ushort)i);
            }
        }

        /// <summary>
        /// Handles Input and Spell-Cooldowns
        /// </summary>
        private void Update()
        {
            for (int i = 0; i < spellCooldown.Length; i++)
                spellCooldown[i] = Mathf.Clamp(spellCooldown[i] - Time.deltaTime, 0, float.MaxValue);
            currScrollTimeout = Mathf.Clamp(currScrollTimeout - Time.deltaTime, 0, float.MaxValue);
            InputState input = player.InputManager.State;
            if (input.SelectSpell != 0) // Spell-Selection
            {
                if (input.SelectSpell < InputState.SpellSelection.SelectPrevious) // Selection by Index
                    SelectSpell((ushort)(input.SelectSpell - 1));
                else if (input.SelectSpell == InputState.SpellSelection.SelectPrevious)
                    SelectPreviousSpell();
                else if (input.SelectSpell == InputState.SpellSelection.SelectNext)
                    SelectNextSpell();
            }
            if (input.Cast) // Spell-Casting
            {
                if (input.CastIndex.HasValue) // Controller
                    CastSpell((ushort)input.CastIndex.Value);
                else // Keyboard
                    CastSpell(); // Cast selected Spell
            }
        }
        #endregion

        #region Private
        /// <summary>
        /// Casts currently selected Spell
        /// </summary>
        private void CastSpell()
        {
            //If the player is allowed to shoot
            if (!GameManager.Instance.Paused && spellCooldown[selectedSpellIndex] <= 0)
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
        /// <summary>
        /// Casts Spell by Index (If Spell is Equipped for Index)
        /// </summary>
        /// <param name="index">Index for Spell (0-4)</param>
        private void CastSpell(ushort index)
        {
            if (selectedSpells.Length >= index && selectedSpells[index] != null)
            {
                selectedSpellIndex = index;
                CastSpell();
            }
        }
        #endregion
        #endregion
    }
}