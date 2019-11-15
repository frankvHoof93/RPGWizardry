using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [SerializeField]
    private WalkDirection lastDirection;
    private Animator animator;

    //incoming movement values from the inputmanager
    public Vector3 inputMovement;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //InputManager.onMovement += Movement;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement(inputMovement);
    }

    //move character
    private void Movement(Vector3 movement)
    {
        //send values to the animator so it can decide what animation to show
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.magnitude);

        //big complicated animation algorhythm
        if (!(movement.x.Equals(0) && movement.y.Equals(0)))
        {
            if (movement.x > 0 && movement.x > movement.y)
            {
                //east
                animator.SetInteger("LastDirection", 0);
                lastDirection = WalkDirection.East;
            }
            else if (movement.y > 0 && movement.y > movement.x)
            {
                //north
                animator.SetInteger("LastDirection", 1);
                lastDirection = WalkDirection.North;
            }
            else if (movement.x < 0 && movement.x < movement.y)
            {
                //west
                animator.SetInteger("LastDirection", 2);
                lastDirection = WalkDirection.West;
            }
            else if (movement.y < 0 && movement.y < movement.x)
            {
                //south
                animator.SetInteger("LastDirection", 3);
                lastDirection = WalkDirection.South;
            }
        }

        //actually move the character
        Vector3 adjustedMovement = transform.position + movement * Time.deltaTime;
        adjustedMovement.z = adjustedMovement.y;
        transform.position = adjustedMovement;
    }
}
