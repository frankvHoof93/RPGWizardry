using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Sorcery.Spells
{
    public abstract class Projectile : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Stats (to be set by a scriptableobject?)
        /// </summary>
        [SerializeField]
        protected int movementSpeed = 2;
        [SerializeField]
        protected int range = 2;
        public float Cooldown = 0.5f;
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Move forward based on the subclass' instantiation of Move
        /// </summary>
        void FixedUpdate()
        {
            Move();
        }

        /// <summary>
        /// When the spell comes in contact with something
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Effect(collision);
        }
        #endregion
        #region Virtual
        /// <summary>
        /// Basic movement that can be edited by a subclass
        /// </summary>
        protected virtual void Move()
        {
            transform.position += transform.up * Time.deltaTime * movementSpeed;
        }

        /// <summary>
        /// Applies the spell's effect to the colliding object (usually damage)
        /// </summary>
        /// <param name="collision"></param>
        protected virtual void Effect(Collider2D collision)
        {
            //oh man i can feel the effect
            Destroy(gameObject);
        }
        #endregion
        #endregion
    }
}
