using nl.SWEG.Willow.Entities.Stats;
using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.UI.CameraEffects.Opacity;
using nl.SWEG.Willow.UI.Popups;
using nl.SWEG.Willow.Utils;
using nl.SWEG.Willow.Utils.Functions;
using UnityEngine;
using static nl.SWEG.Willow.Entities.Enemies.EnemyData;

namespace nl.SWEG.Willow.Entities.Enemies
{
    /// <summary>
    /// Base Class for an Enemy
    /// </summary>
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
        /// Opacity-Radius in Pixels (for 720p resolution)
        /// </summary>
        public virtual float OpacityRadius => data?.OpacityRadius ?? 0;
        /// <summary>
        /// Priority for rendering Opacity
        /// </summary>
        public int OpacityPriority => data?.OpacityPriority ?? 1;
        /// <summary>
        /// Opacity-Offset from Transform (in World-Space)
        /// </summary>
        public Vector2 OpacityOffset => data?.OpacityOffset ?? Vector2.zero;
        #endregion

        #region Protected
        /// <summary>
        /// Animator for this Enemy
        /// </summary>
        protected Animator enemyAnimator;
        /// <summary>
        /// Renderer for this Enemy
        /// </summary>
        protected Renderer enemyRenderer;
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Default values for this Enemy
        /// </summary>
        [SerializeField]
        [Tooltip("Default values for this Enemy")]
        protected EnemyData data;
        /// <summary>
        /// LayerMask for Attacks performed by this Enemy
        /// </summary>
        [SerializeField]
        [Tooltip("LayerMask for Attacks performed by this Enemy")]
        protected LayerMask attackCollisionMask;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Private
        /// <summary>
        /// Time at which Updates are enabled for this Enemy
        /// <para>
        /// This time is determined at Start by grabbing a random Cooldown-Value from the EnemyData
        /// </para>
        /// </summary>
        private float enableTime;
        /// <summary>
        /// Event fired when Enemy Dies
        /// </summary>
        private event Die Death;
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// No Implementation (Enemies cannot Heal)
        /// </summary>
        /// <param name="amount">N.A.</param>
        /// <returns>False (Always)</returns>
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
            if (Health == 0)
                return; // Already Dead. Hit while animating death
            if (amount >= Health)
            {
                Health = 0;
                Die();
            }
            else
            {
                Health -= amount;
            }
            enemyRenderer.SetSpriteColor(Color.red);
            PopupFactory.CreateDamageUI(transform.position, amount, enemyRenderer, Color.red);
            StartCoroutine(CoroutineMethods.RunDelayed(() => enemyRenderer.SetSpriteColor(Color.white), .1f));
        }
        /// <summary>
        /// Adds Listener to Death-Event
        /// </summary>
        /// <param name="listener">Listener to Add</param>
        public void AddDeathListener(Die listener)
        {
            Death += listener;
        }
        #endregion

        #region Unity
        /// <summary>
        /// Grabs references to Animator & Renderer
        /// </summary>
        private void Awake()
        {
            enemyAnimator = GetComponent<Animator>();
            enemyRenderer = GetComponent<Renderer>();
        }

        /// <summary>
        /// Sets default values from Data
        /// </summary>
        protected virtual void Start()
        {
            Health = data.Health;
            enableTime = Time.time + data.SpawnCooldown.Random;
        }

        /// <summary>
        /// Runs Update-Implementation (if Player Exists)
        /// <para>
        /// Animates Enemy (Always)
        /// </para>
        /// </summary>
        private void Update()
        {
            if (PlayerManager.Exists && Time.time >= enableTime
                && GameManager.Exists && !GameManager.Instance.Paused) // Update only when playing
                UpdateEnemy(PlayerManager.Instance);
            AnimateEnemy(); // Animate Always
        }
        #endregion

        #region Protected
        /// <summary>
        /// Updates Enemy
        /// </summary>
        /// <param name="player">Reference To Player</param>
        protected abstract void UpdateEnemy(PlayerManager player);
        /// <summary>
        /// Animates Enemy
        /// </summary>
        protected abstract void AnimateEnemy();
        /// <summary>
        /// Handles Enemy Death (e.g. Animation)
        /// </summary>
        protected abstract void OnDeath();
        #endregion

        #region Private
        /// <summary>
        /// Kills this Enemy, dropping Loot and calling OnDeath
        /// </summary>
        private void Die()
        {
            float rng = Random.Range(0f, 1f);
            LootTable loot = data.Loot;
            LootSpawn spawn = loot.dust; // Dust
            if (spawn.amount > 0 && spawn.chance >= rng)
                LootSpawner.Instance.SpawnLoot(Collectables.Collectables.Dust, transform.position, spawn.amount);
            spawn = loot.gold; // Gold
            if (spawn.amount > 0 && spawn.chance >= rng)
                LootSpawner.Instance.SpawnLoot(Collectables.Collectables.Gold, transform.position, spawn.amount);
            spawn = loot.potion; // Potion
            if (spawn.amount > 0 && spawn.chance >= rng)
                LootSpawner.Instance.SpawnLoot(Collectables.Collectables.Potion, transform.position, spawn.amount);
            // Spawning of SpellPage handled seperately by BookEnemy
            OnDeath();
        }
        /// <summary>
        /// Fires Death-Event for Enemy
        /// </summary>
        private void OnDestroy()
        {
            Death?.Invoke();
        }
        #endregion
        #endregion
    }
}