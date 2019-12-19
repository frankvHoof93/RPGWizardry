using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Entities.Stats;
using nl.SWEG.RPGWizardry.GameWorld;
using UnityEngine;
using static nl.SWEG.RPGWizardry.Entities.Enemies.EnemyData;
using nl.SWEG.RPGWizardry.GameWorld.OpacityManagement;
using nl.SWEG.RPGWizardry.UI;

namespace nl.SWEG.RPGWizardry.Entities.Enemies
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Renderer))]
    public abstract class AEnemy : MonoBehaviour, IHealth, IOpacity
    {
        #region Variables
        #region Public
        /// <summary>
        /// Current Health for this Enemy
        /// </summary>
        public ushort Health { get; private set; }
        /// <summary>
        /// Opacity-Radius in Pixels (for 720p)
        /// </summary>
        public float OpacityRadius => data?.OpacityRadius ?? 0;
        /// <summary>
        /// Priority for rendering Opacity
        /// </summary>
        public int OpacityPriority => data?.OpacityPriority ?? 1;
        /// <summary>
        /// Opacity-Offset from Transform (in World-Space)
        /// </summary>
        public Vector2 OpacityOffset => data?.OpacityOffset ?? Vector2.zero;
        public delegate void Kill();
        public Kill Killed;
        #endregion

        #region Protected
        /// <summary>
        /// Default values for this Enemy
        /// </summary>
        [SerializeField]
        [Tooltip("Default values for this Enemy")]
        protected EnemyData data;
        /// <summary>
        /// Animator for Enemy
        /// </summary>
        protected Animator animator;
        protected Renderer renderer;
        /// <summary>
        /// LayerMask for Attacks
        /// </summary>
        [SerializeField]
        [Tooltip("LayerMask for Attacks")]
        protected LayerMask attackCollisionMask;
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
                PopupFactory.CreateDamageUI(transform.position, amount, renderer, Color.green);
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
            renderer = GetComponent<Renderer>();
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
            if (PlayerManager.Exists && Time.time >= enableTime)
                UpdateEnemy(PlayerManager.Instance);
            AnimateEnemy();
        }
        #endregion

        #region Protected
        protected abstract void UpdateEnemy(PlayerManager player);
        protected abstract void AnimateEnemy();
        protected abstract void OnDeath();
        #endregion

        #region Private
        /// <summary>
        /// Kills this Enemy, running Animations & dropping Loot
        /// </summary>
        private void Die()
        {
            PopupFactory.CreateDamageUI(transform.position, Health, renderer, Color.green);
            float rng = Random.Range(0f, 1f);
            LootTable loot = data.Loot;
            LootSpawn spawn = loot.dust;
            if (spawn.amount > 0 && spawn.chance >= rng)
                LootSpawner.Instance.SpawnLoot(Collectables.Collectables.Dust, transform.position, spawn.amount);
            spawn = loot.gold;
            if (spawn.amount > 0 && spawn.chance >= rng)
                LootSpawner.Instance.SpawnLoot(Collectables.Collectables.Gold, transform.position, spawn.amount);
            spawn = loot.potion;
            if (spawn.amount > 0 && spawn.chance >= rng)
                LootSpawner.Instance.SpawnLoot(Collectables.Collectables.Potion, transform.position, spawn.amount);

            // TODO: Death Animation & Audio
            OnDeath();
            transform.parent = null;
        }

        private void OnDestroy()
        {
            if (Killed != null)
            {
                Killed();
            }
        }
        #endregion
        #endregion
    }
}