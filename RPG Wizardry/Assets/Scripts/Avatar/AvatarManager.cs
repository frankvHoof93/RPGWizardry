using nl.SWEG.RPGWizardry.Entities.Stats;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using System;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Avatar
{
    public class AvatarManager : SingletonBehaviour<AvatarManager>, IHealth
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
        [SerializeField]
        private SpriteRenderer bookRenderer;
        #endregion

        #region Editor
        /// <summary>
        /// Max Health for Player
        /// </summary>
        [SerializeField]
        [Tooltip("Max Health for Player")]
        private ushort maxHealth = 100;
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

        #region Events
        /// <summary>
        /// Adds a Listener to the HealthChangeEvent
        /// </summary>
        /// <param name="listener">Listener to Add</param>
        public void AddHealthChangeListener(OnHealthChange listener)
        {
            healthChangeEvent += listener;
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
        /// Sets Health to maxHealth
        /// </summary>
        private void Start()
        {
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