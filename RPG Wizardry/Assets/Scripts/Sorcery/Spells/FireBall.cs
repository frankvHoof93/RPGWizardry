using UnityEngine;

namespace nl.SWEG.RPGWizardry.Sorcery.Spells
{
    public class FireBall : Projectile
    {
        protected override void Effect(Collider2D collision)
        {
            //EXPLODE
            base.Effect(collision);
        }
    }
}