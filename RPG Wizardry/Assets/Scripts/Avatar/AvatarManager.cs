using System;
using nl.SWEG.RPGWizardry.Entities.Stats;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Avatar
{
    public class AvatarManager : SingletonBehaviour<AvatarManager>, IHealth
    {
        #region Variables
        /// <summary>
        /// Player Health
        /// </summary>
        public ushort Health { get; private set; }
        /// <summary>
        /// Max Health for Player
        /// </summary>
        [SerializeField]
        [Tooltip("Max Health for Player")]
        private ushort maxHealth = 100;
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
            Health -= amount;
            if (Health <= 0)
                Die();
        }

        /// <summary>
        /// Heals Player
        /// </summary>
        /// <param name="amount">Amount of Healing to inflict</param>
        public bool Heal(ushort amount)
        {
            if (Health == maxHealth)
                return false;
            Health += amount;
            if (Health > maxHealth)
                Health = maxHealth;
            return true;
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