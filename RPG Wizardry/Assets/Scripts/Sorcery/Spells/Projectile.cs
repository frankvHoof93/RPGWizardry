using nl.SWEG.RPGWizardry.Entities.Stats;
using nl.SWEG.RPGWizardry.Utils.Functions;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Sorcery.Spells
{
    public abstract class Projectile : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Data for Spell
        /// </summary>
        protected SpellData data;
        /// <summary>
        /// Mask of layer containing enemies
        /// </summary>
        protected LayerMask targetLayer;
        /// <summary>
        /// Mask of layer containing walls and other obstructions
        /// </summary>
        [SerializeField]
        private LayerMask wallLayer;
        /// <summary>
        /// Combined layermask for all things to collide with
        /// </summary>
        private LayerMask collisionLayer;
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
        private void Start()
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
            //oh man i can feel the effect
            collision.gameObject.GetComponent<IHealth>()?.Damage(data.Damage);
            Destroy(gameObject); // TODO: Animation?
        }
        #endregion
        #endregion
    }
}