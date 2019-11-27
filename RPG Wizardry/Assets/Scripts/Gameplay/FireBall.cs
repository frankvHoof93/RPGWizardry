using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Projectile
{
    protected override void Effect(Collider2D collision)
    {
        //EXPLODE
        Debug.Log("Fireball explode");
        base.Effect(collision);
    }
}
