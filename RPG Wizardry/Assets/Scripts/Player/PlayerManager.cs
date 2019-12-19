using nl.SWEG.RPGWizardry.Player.Inventory;
using nl.SWEG.RPGWizardry.Entities.Stats;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using System.Collections;
using UnityEngine;
using nl.SWEG.RPGWizardry.Player.Combat;
using nl.SWEG.RPGWizardry.Player.PlayerInput;
using nl.SWEG.RPGWizardry.GameWorld.OpacityManagement;
using nl.SWEG.RPGWizardry.GameWorld;
using System;
using nl.SWEG.RPGWizardry.UI;

namespace nl.SWEG.RPGWizardry.Player
{
    [RequireComponent(typeof(PlayerInventory), typeof(CastingManager), typeof(InputManager))]
    [RequireComponent(typeof(Renderer))]
    public class PlayerManager : SingletonBehaviour<PlayerManager>, IHealth, IOpacity
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
        /// <summary>
        /// Opacity-Radius in Pixels (for 720p)
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
        #endregion

        #region Private
        /// <summary>
        /// Event Raised when Health changes
        /// </summary>
        private event OnHealthChange healthChangeEvent;
        private Renderer renderer;
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
            Health = (ushort)Mathf.Clamp(Health - amount, 0, Health);
            PopupFactory.CreateDamageUI(transform.position, amount, renderer, Color.red, 50);
            healthChangeEvent?.Invoke(Health, maxHealth, (short)-amount);
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
            PopupFactory.CreateDamageUI(transform.position, amount, renderer, Color.green, 50);
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
            CastingManager = GetComponent<CastingManager>();
            InputManager = GetComponent<InputManager>();
            renderer = GetComponent<Renderer>();
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
            GetComponent<Collider2D>().enabled = false;
            if (CameraManager.Exists)
            {
                StartCoroutine(EndGame());
            }
        }

        private IEnumerator EndGame()
        {
            CameraManager.instance.Fade(0.7f, 0, 2f);
            yield return new WaitForSeconds(2);
            Application.Quit();
        }
        #endregion
        #endregion
    }
}