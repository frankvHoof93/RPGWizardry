using nl.SWEG.RPGWizardry.Entities.Stats;
using nl.SWEG.RPGWizardry.GameWorld.OpacityManagement;
using nl.SWEG.RPGWizardry.Utils.Functions;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Sorcery.Spells
{
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
        /// Opacity-Radius in Pixels (for 720p)
        /// </summary>
        [SerializeField]
        [Range(1, 10000)]
        [Tooltip("Opacity-Radius in Pixels (for 720p)")]
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
        /// Combined layermask for all things to collide with
        /// </summary>
        protected LayerMask collisionLayer;
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
        }
        #endregion

        #region Unity
        /// <summary>
        /// Combines layermasks
        /// </summary>
        protected virtual void Start()
        {
            collisionLayer = targetLayer | wallLayer;
        }

        /// <summary>
        /// Move forward based on the subclass' instantiation of Move
        /// </summary>
        private void FixedUpdate()
        {
            Move();
        }

        /// <summary>
        /// When the spell comes in contact with something
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collisionLayer.HasLayer(collision.gameObject.layer))
                Effect(collision);
        }
        #endregion

        #region Protected
        /// <summary>
        /// Basic movement that can be edited by a subclass
        /// </summary>
        protected virtual void Move()
        {
            transform.position += transform.up * Time.deltaTime * data.ProjectileSpeed;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        }

        /// <summary>
        /// Applies the spell's effect to the colliding object (usually damage)
        /// </summary>
        /// <param name="collision"></param>
        protected virtual void Effect(Collider2D collision)
        {
            GetComponent<Collider2D>().enabled = false;
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