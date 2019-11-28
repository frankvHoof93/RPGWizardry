using UnityEngine;

namespace nl.SWEG.RPGWizardry.Sorcery.Spells
{
    public class Projectile : MonoBehaviour
    {

        #region Variables
        /// <summary>
        /// Stats (to be set by a scriptableobject?)
        /// </summary>
        [SerializeField]
        private int movementSpeed = 2;
        [SerializeField]
        private int range = 2;
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
        private void OnCollisionEnter(Collision collision)
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
        protected virtual void Effect(Collision collision)
        {
            //oh man i can feel the effect
        }
        #endregion
        #endregion
    }
}