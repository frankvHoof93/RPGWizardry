using nl.SWEG.Willow.Entities.Stats;
using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.UI;
using nl.SWEG.Willow.UI.CameraEffects.Opacity;
using nl.SWEG.Willow.Utils.Functions;
using UnityEngine;

namespace nl.SWEG.Willow.Sorcery.Spells
{
    /// <summary>
    /// Base Class for Projectiles
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public abstract class Projectile : MonoBehaviour, IOpacity
    {
        #region Variables
        #region Public
        /// <summary>
        /// Opacity-Radius in Pixels (for 720p)
        /// </summary>
        public float OpacityRadius => opacityRadius;
        /// <summary>
        /// Priority for rendering Opacity
        /// </summary>
        public int OpacityPriority => opacityPriority;
        /// <summary>
        /// Opacity-Offset from Transform (in World-Space)
        /// </summary>
        public Vector2 OpacityOffset => opacityOffset;
        #endregion

        #region Editor
        /// <summary>
        /// Mask of layer containing walls and other obstructions
        /// </summary>
        [SerializeField]
        [Tooltip("Mask of layer containing walls and other obstructions")]
        private LayerMask wallLayer;
        [Header("Opacity")]
        /// <summary>
        /// Opacity-Radius in Pixels (for 720p-Resolution)
        /// </summary>
        [SerializeField]
        [Range(1, 10000)]
        [Tooltip("Opacity-Radius in Pixels (for 720p-Resolution)")]
        private int opacityPriority = 2;
        /// <summary>
        /// Priority for rendering Opacity
        /// </summary>
        [SerializeField]
        [Tooltip("Priority for rendering Opacity")]
        private float opacityRadius = 35f;
        /// <summary>
        /// Opacity-Offset from Transform (in World-Space)
        /// </summary>
        [SerializeField]
        [Tooltip("Opacity-Offset from Transform (in World-Space)")]
        private Vector2 opacityOffset;
        #endregion

        #region Protected
        /// <summary>
        /// Data for Spell
        /// </summary>
        protected SpellData data;
        /// <summary>
        /// Mask of layer containing enemies
        /// </summary>
        protected LayerMask targetLayer;
        /// <summary>
        /// Combined layermask of all objects to collide with
        /// </summary>
        protected LayerMask collisionLayer;
        #endregion

        #region Private
        /// <summary>
        /// Collider for Projectile
        /// </summary>
        private Collider2D coll;
        /// <summary>
        /// Current LifeTime for Projectile. Used for Destruction
        /// </summary>
        private float lifeTime;
        #endregion
        #endregion

        #region Methods
        #region Internal
        /// <summary>
        /// Sets Speed and TargetingLayer
        /// </summary>
        /// <param name="speed">Speed for Projectile</param>
        /// <param name="dmg">Amount of daamage inflicted by Projectile</param>
        /// <param name="targetingLayer">TargetingLayer(s) for Projectile</param>
        internal void SetData(SpellData spellData, LayerMask targetingLayer)
        {
            data = spellData;
            targetLayer = targetingLayer;
            collisionLayer = targetLayer | wallLayer;
        }
        #endregion

        #region Unity
        /// <summary>
        /// Grabs reference to Collider
        /// </summary>
        private void Awake()
        {
            coll = GetComponent<Collider2D>();
        }

        /// <summary>
        /// Combines layermasks
        /// </summary>
        protected virtual void Start()
        {
            collisionLayer = targetLayer | wallLayer;
            if (data.SpawnClip != null)
                AudioManager.Instance.PlaySound(data.SpawnClip);
        }

        /// <summary>
        /// Moves Projectile
        /// </summary>
        private void FixedUpdate()
        {
            if (GameManager.Exists && !GameManager.Instance.Paused)
                Move();
        }

        /// <summary>
        /// Handles Collisions with Objects
        /// </summary>
        /// <param name="collision">Collision that occurred</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collisionLayer.HasLayer(collision.gameObject.layer))
                Effect(collision);
        }
        #endregion

        #region Protected
        /// <summary>
        /// Basic movement that can be expanded upon by a subclass
        /// </summary>
        protected virtual void Move()
        {
            transform.position += transform.up * Time.deltaTime * data.ProjectileSpeed;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
            lifeTime += Time.deltaTime;
            if (lifeTime >= data.LifeTime)
                Destroy(gameObject);
        }

        /// <summary>
        /// Applies the spell's effect to the colliding object (usually damage)
        /// </summary>
        /// <param name="collision">Collision that occurred</param>
        protected virtual void Effect(Collider2D collision)
        {
            //play impact sound
            if (data.ImpactClip != null)
                AudioManager.Instance.PlaySound(data.ImpactClip);
            coll.enabled = false;
            //apply knockback
            Rigidbody2D body = collision.gameObject.GetComponent<Rigidbody2D>();
            body.AddForce(transform.up * data.Knockback);
            //oh man i can feel the effect
            collision.gameObject.GetComponent<IHealth>()?.Damage(data.Damage);
            Destroy(gameObject); // TODO: Animation?
        }
        #endregion
        #endregion
    }
}