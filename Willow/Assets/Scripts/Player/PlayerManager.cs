using nl.SWEG.Willow.Entities.Stats;
using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.Player.Combat;
using nl.SWEG.Willow.Player.Inventory;
using nl.SWEG.Willow.Player.Movement;
using nl.SWEG.Willow.Player.PlayerInput;
using nl.SWEG.Willow.UI.CameraEffects;
using nl.SWEG.Willow.UI.CameraEffects.Opacity;
using nl.SWEG.Willow.UI.Popups;
using nl.SWEG.Willow.Utils.Behaviours;
using nl.SWEG.Willow.Utils.Functions;
using UnityEngine;

namespace nl.SWEG.Willow.Player
{
    /// <summary>
    /// Main Controller for Player-Avatar
    /// <para>
    /// Handles Health & Death, and holds references to Player-Scripts (e.g. Inventory)
    /// </para>
    /// </summary>
    [RequireComponent(typeof(PlayerInventory), typeof(CastingManager), typeof(InputManager))]
    [RequireComponent(typeof(Renderer), typeof(Collider2D))]
    public class PlayerManager : SingletonBehaviour<PlayerManager>, IHealth, IOpacity
    {
        #region Variables
        #region Public
        /// <summary>
        /// Player Health
        /// </summary>
        public ushort Health { get; private set; }
        /// <summary>
        /// Renderer for Greg, necessary for bookerang spell
        /// </summary>
        public SpriteRenderer BookRenderer => bookRenderer;
        /// <summary>
        /// Opacity-Radius in Pixels (for 720p-Resolution)
        /// </summary>
        public float OpacityRadius => opacityRadius;
        /// <summary>
        /// Priority for rendering Opacity
        /// </summary>
        public int OpacityPriority => 0; // Highest Priority
        /// <summary>
        /// Opacity-Offset from Transform (in World-Space)
        /// </summary>
        public Vector2 OpacityOffset => opacityOffset;
        #endregion

        #region Internal
        /// <summary>
        /// Inventory for Player
        /// </summary>
        internal PlayerInventory Inventory { get; private set; }
        /// <summary>
        /// CastingManager for Player
        /// </summary>
        internal CastingManager CastingManager { get; private set; }
        /// <summary>
        /// InputManager for Player
        /// </summary>
        internal InputManager InputManager { get; private set; }
        /// <summary>
        /// MovementManager for Player
        /// </summary>
        internal MovementManager MovementManager { get; private set; }
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

        [Header("Opacity")]
        /// <summary>
        /// Opacity-Radius in Pixels (for 720p)
        /// </summary>
        [SerializeField]
        [Tooltip("Opacity-Radius in Pixels (for 720p)")]
        private float opacityRadius = 30f;
        /// <summary>
        /// Opacity-Offset from Transform (in World-Space
        /// </summary>
        [SerializeField]
        [Tooltip("Opacity-Offset from Transform (in World-Space)")]
        private Vector2 opacityOffset = new Vector2(0f, -30f);
        /// <summary>
        /// Amount of FRAMES Player is invincible for after being damaged
        /// </summary>
        [SerializeField]
        [Tooltip("Amount of FRAMES Player is invincible for after being damaged")]
        private uint invincibilityFrames = 60;

        [Header("Layers")]
        /// <summary>
        /// Layer that contains health potions
        /// </summary>
        [SerializeField]
        private LayerMask healthPotionLayer;
        #endregion

        #region Private
        /// <summary>
        /// Event Raised when Health changes
        /// </summary>
        private event OnHealthChange healthChangeEvent;
        /// <summary>
        /// Renderer for Player
        /// </summary>
        private Renderer playerRenderer;
        /// <summary>
        /// Collider for Player
        /// </summary>
        private Collider2D coll;
        /// <summary>
        /// Whether the Player is currently Invincible
        /// </summary>
        private bool isInvincible;
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
            if (!isInvincible)
            {
                isInvincible = true;
                if (amount >= Health)
                    Die();
                // Stop ignoring HealthPotions, 
                Physics2D.IgnoreLayerCollision(gameObject.layer, (int)Mathf.Log(healthPotionLayer.value, 2), false);
                Health = (ushort)Mathf.Clamp(Health - amount, 0, Health);
                PopupFactory.CreateDamageUI(transform.position, amount, playerRenderer, Color.red, 50);
                healthChangeEvent?.Invoke(Health, maxHealth, (short)-amount);
                // Tween to Red
                LeanTween.value(gameObject, col => playerRenderer.SetSpriteColor(col), Color.white, Color.red, (invincibilityFrames / 60f) / 6f)
                    .setLoopPingPong(3).setOnComplete(() => isInvincible = false);                
                // Shake Screen
                ScreenShake.Instance.Shake(0.5f, 0.2f);
                // Stun Player
                MovementManager.Stun(0.2f);
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
            if (Health == maxHealth)
            {
                // Ignore HealthPotions, since Health is full
                Physics2D.IgnoreLayerCollision(gameObject.layer, (int)Mathf.Log(healthPotionLayer.value, 2), true);
            }
            healthChangeEvent?.Invoke(Health, maxHealth, (short)amount);
            PopupFactory.CreateDamageUI(transform.position, amount, playerRenderer, Color.green, 50);
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
        /// Grabs references and sets Health
        /// </summary>
        protected override void Awake()
        {
            Inventory = GetComponent<PlayerInventory>();
            CastingManager = GetComponent<CastingManager>();
            InputManager = GetComponent<InputManager>();
            MovementManager = GetComponent<MovementManager>();
            playerRenderer = GetComponent<Renderer>();
            coll = GetComponent<Collider2D>();
            base.Awake();
            Health = maxHealth;
            // Ignore HealthPotions, since Health is full
            Physics2D.IgnoreLayerCollision(gameObject.layer, (int)Mathf.Log(healthPotionLayer.value, 2), true);
        }
        #endregion

        #region Private
        /// <summary>
        /// Performs death-animation for player, and starts Run-End
        /// </summary>
        private void Die()
        {
            coll.enabled = false;
            if (CameraManager.Exists)
                CameraManager.Instance.Fade(0.7f, 0, 2f);
            GameManager.Instance.EndGame(true);
        }
        #endregion
        #endregion
    }
}