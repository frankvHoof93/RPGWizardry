using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField]
    GameObject Player;
    public float horizontal, vertical;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = transform.parent.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //float angle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg - 90f;
        //rb.rotation = angle;
    }
}
