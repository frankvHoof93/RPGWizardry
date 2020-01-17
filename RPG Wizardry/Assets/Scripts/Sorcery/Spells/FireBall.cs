using nl.SWEG.RPGWizardry.Audio;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Sorcery.Spells
{
    public class FireBall : Projectile
    {
        /// <summary>
        /// The object that contains the splash animation and collider
        /// </summary>
        [SerializeField]
        private GameObject SplashObject;

        /// <summary>
        /// In addition to deleting self, spawn an explosion that does splash damage
        /// </summary>
        /// <param name="collision"></param>
        protected override void Effect(Collider2D collision)
        {
            //EXPLODE
            GameObject splash = Instantiate(SplashObject, transform.position, transform.rotation);
            splash.transform.localScale = transform.localScale; // Scale relative to Fireball-size
            base.Effect(collision);
        }
    }
}