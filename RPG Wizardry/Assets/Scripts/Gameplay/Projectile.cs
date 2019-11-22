using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    
    private int movementSpeed = 2;
    public int Cooldown = 1;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.up * Time.deltaTime * movementSpeed;
    }
}
