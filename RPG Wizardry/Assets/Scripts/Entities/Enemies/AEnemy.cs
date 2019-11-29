using nl.SWEG.RPGWizardry.Avatar;
using nl.SWEG.RPGWizardry.Entities.Stats;
using nl.SWEG.RPGWizardry.GameWorld;
using UnityEngine;
using static nl.SWEG.RPGWizardry.Entities.Enemies.EnemyData;

namespace nl.SWEG.RPGWizardry.Entities.Enemies
{
    [RequireComponent(typeof(Animator))]
    public abstract class AEnemy : MonoBehaviour, IHealth
    {
        #region Variables
        #region Public
        /// <summary>
        /// Current Health for this Enemy
        /// </summary>
        public ushort Health { get; private set; }
        #endregion

        #region Protected
        /// <summary>
        /// Default values for this Enemy
        /// </summary>
        [SerializeField]
        protected EnemyData data;
        /// <summary>
        /// Animator for Enemy
        /// </summary>
        protected Animator animator;
        #endregion

        #region Private
        /// <summary>
        /// Time at which Updates are enabled for this Enemy. This time is determined at Start by grabbing a random Cooldown-Value from the EnemyData
        /// </summary>
        private float enableTime;
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
            if (amount >= Health)
                Die();
            else
            {
                Health -= amount;
                // TODO: Animation?
            }
        }
        #endregion

        #region Unity
        /// <summary>
        /// Grabs reference to Animator
        /// </summary>
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Sets default values
        /// </summary>
        protected virtual void Start()
        {
            Health = data.Health;
            enableTime = Time.time + data.SpawnCooldown.Random;
        }

        /// <summary>
        /// Runs Update-Implementation if Player Exists
        /// </summary>
        private void Update()
        {
            if (AvatarManager.Exists && Time.time >= enableTime)
                UpdateEnemy(AvatarManager.Instance);
            AnimateEnemy();
        }
        #endregion

        #region Protected
        protected abstract void UpdateEnemy(AvatarManager player);
        protected abstract void AnimateEnemy();
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