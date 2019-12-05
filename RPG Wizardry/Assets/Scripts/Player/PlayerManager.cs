using nl.SWEG.RPGWizardry.Player.Inventory;
using nl.SWEG.RPGWizardry.Entities.Stats;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using System;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Player
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerManager : SingletonBehaviour<PlayerManager>, IHealth
    {
        #region Variables
        #region Public
        /// <summary>
        /// Player Health
        /// </summary>
        public ushort Health { get; private set; }

        /// <summary>
        /// Renderer of the "crosshair" book, necessary for bookerang spell
        /// </summary>
        public SpriteRenderer BookRenderer => bookRenderer;
        #endregion

        #region Internal
        /// <summary>
        /// Inventory for Player
        /// </summary>
        internal PlayerInventory Inventory { get; private set; }
        #endregion

        #region Editor
        /// <summary>
        /// Max Health for Player
        /// </summary>
        [SerializeField]
        [Tooltip("Max Health for Player")]
        private ushort maxHealth = 100;
        /// <summary>
        /// Renderer for Greg
        /// </summary>
        [SerializeField]
        [Tooltip("Renderer for Greg")]
        private SpriteRenderer bookRenderer;
        #endregion

        #region
        /// <summary>
        /// Event Raised when Health changes
        /// </summary>
        private event OnHealthChange healthChangeEvent;
        #endregion
        #endregion

        #region Methods
        #region Public
        #region Stats
        /// <summary>
        /// Damages Player
        /// </summary>
        /// <param name="amount">Amount of Damage to inflict</param>
        public void Damage(ushort amount)
        {
            if (amount >= Health)
                Die();
            else
            {
                Health = (ushort)Mathf.Clamp(Health - amount, 0, Health);
                healthChangeEvent?.Invoke(Health, maxHealth, (short)-amount);
            }
        }

        /// <summary>
        /// Heals Player
        /// </summary>
        /// <param name="amount">Amount of Healing to inflict</param>
        public bool Heal(ushort amount)
        {
            if (Health == maxHealth)
                return false;
            Health = (ushort)Mathf.Clamp(Health + amount, Health, maxHealth);
            healthChangeEvent?.Invoke(Health, maxHealth, (short)amount);
            return true;
        }
        #endregion

        #region EventListeners
        /// <summary>
        /// Adds a Listener to the HealthChangeEvent
        /// </summary>
        /// <param name="listener">Listener to Add</param>
        public void AddHealthChangeListener(OnHealthChange listener)
        {
            healthChangeEvent += listener;
            // Set Initial Value
            listener.Invoke(Health, maxHealth, 0);
        }
        /// <summary>
        /// Removes a Listener from the HealthChangeEvent
        /// </summary>
        /// <param name="listener">Listener to Remove</param>
        public void RemoveHealthChangeListener(OnHealthChange listener)
        {
            healthChangeEvent -= listener;
        }
        #endregion
        #endregion

        #region Unity
        /// <summary>
        /// Grabs reference to Inventory and sets Health
        /// </summary>
        protected override void Awake()
        {
            Inventory = GetComponent<PlayerInventory>();
            base.Awake();
            Health = maxHealth;
        }
        #endregion

        #region Private
        /// <summary>
        /// Performs death-animation for player, and respawns
        /// </summary>
        private void Die()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}