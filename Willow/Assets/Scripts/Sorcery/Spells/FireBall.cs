using UnityEngine;

namespace nl.SWEG.Willow.Sorcery.Spells
{
    /// <summary>
    /// A FireBall is a Ball of fire that travels through the air until it hits an object, then explodes
    /// </summary>
    public class FireBall : Projectile
    {
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Explosion-Object
        /// </summary>
        [SerializeField]
        [Tooltip("Explosion-Object")]
        private GameObject splashObject;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables

        /// <summary>
        /// Spawns an explosion
        /// </summary>
        /// <param name="collision">Collision that occurred</param>
        protected override void Effect(Collider2D collision)
        {
            //EXPLODE
            GameObject splash = Instantiate(splashObject, transform.position, transform.rotation);
            splash.transform.localScale = transform.localScale; // Scale relative to Fireball-size
            base.Effect(collision);
        }

        /// <summary>
        /// Called when Projectile hits a Wall
        /// </summary>
        /// <param name="collision">Wall that was hit</param>
        protected override void HitWall(Collider2D collision)
        {
            Effect(collision);
        }
    }
}