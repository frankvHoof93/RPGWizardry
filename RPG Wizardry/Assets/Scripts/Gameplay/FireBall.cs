using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Projectile
{
    protected override void Effect(Collision collision)
    {
        //EXPLODE
        base.Effect(collision);
    }
}
