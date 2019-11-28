using nl.SWEG.RPGWizardry.Entities.Stats;
using nl.SWEG.RPGWizardry.GameWorld;
using UnityEngine;
using static nl.SWEG.RPGWizardry.Entities.Enemies.EnemyData;

namespace nl.SWEG.RPGWizardry.Entities.Enemies
{
    public /* abstract */ class Enemy : MonoBehaviour, IHealth
    {
        #region Variables
        #region Public
        /// <summary>
        /// Current Health for this Enemy
        /// </summary>
        public ushort Health { get; private set; }
        #endregion

        #region Private
        /// <summary>
        /// Default values for this Enemy
        /// </summary>
        [SerializeField]
        private EnemyData data;
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// No Implementation (Enemies cannot Heal)
        /// </summary>
        /// <param name="amount">N.A.</param>
        public bool Heal(ushort amount)
        {
            return false; // Enemies cannot heal
        }
        /// <summary>
        /// Damages this Enemy
        /// </summary>
        /// <param name="amount">Amount of Damage to inflict</param>
        public void Damage(ushort amount)
        {
            Health -= amount;
            if (Health <= 0)
                Die();
        }
        #endregion

        #region Unity
        /// <summary>
        /// Sets default values
        /// </summary>
        private void Start()
        {
            Health = data.Health;
        }
        #endregion

        #region Private
        /// <summary>
        /// Kills this Enemy, running Animations & dropping Loot
        /// </summary>
        private void Die()
        {
            float rng = Random.Range(0f, 1f);
            LootTable loot = data.Loot;
            LootSpawn spawn = loot.dust;
            if (spawn.amount > 0 && spawn.chance >= rng)
                LootSpawner.Instance.SpawnLoot(Collectables.Collectables.Dust, transform.position, spawn.amount);
            spawn = loot.gold;
            if (spawn.amount > 0 && spawn.chance >= rng)
                LootSpawner.Instance.SpawnLoot(Collectables.Collectables.Gold, transform.position, spawn.amount);
            spawn = loot.page;
            if (spawn.amount > 0 && spawn.chance >= rng)
                LootSpawner.Instance.SpawnLoot(Collectables.Collectables.Page, transform.position, spawn.amount);
            spawn = loot.potion;
            if (spawn.amount > 0 && spawn.chance >= rng)
                LootSpawner.Instance.SpawnLoot(Collectables.Collectables.Potion, transform.position, spawn.amount);


            // TODO: Death Animation & Audio

            Destroy(gameObject);
        }
        #endregion
        #endregion
    }
}