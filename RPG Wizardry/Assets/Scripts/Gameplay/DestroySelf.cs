using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    /// <summary>
    /// Destroys this object. Called from animationevent
    /// </summary>
    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
