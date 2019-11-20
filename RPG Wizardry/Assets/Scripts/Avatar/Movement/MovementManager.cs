using UnityEngine;

namespace nl.SWEG.RPGWizardry.Avatar.Movement
{
    [RequireComponent(typeof(Animator))]
    public class MovementManager : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Incoming movement values from the inputmanager
        /// </summary>
        public Vector3 movementInput;
        /// <summary>
        /// Animator for Avatar
        /// </summary>
        private Animator animator;
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Grabs Reference to Animator
        /// </summary>
        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Handles Movement based on Input
        /// </summary>
        private void FixedUpdate()
        {
            Movement(movementInput);
        }
        #endregion

        #region Private
        /// <summary>
        /// Moves Avatar based on input
        /// </summary>
        /// <param name="movement">Input data in vector3 format</param>
        private void Movement(Vector3 movement)
        {
            //send values to the animator so it can decide what animation to show
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.magnitude);

            //big complicated animation algorhythm
            if (!(movement.x == 0 && movement.y == 0))
            {
                if (movement.x > 0 && movement.x > movement.y) // East
                    animator.SetInteger("LastDirection", 0);
                else if (movement.y > 0 && movement.y > movement.x) // North
                    animator.SetInteger("LastDirection", 1);
                else if (movement.x < 0 && movement.x < movement.y) // West
                    animator.SetInteger("LastDirection", 2);
                else if (movement.y < 0 && movement.y < movement.x) // South
                    animator.SetInteger("LastDirection", 3);
            }
            //actually move the character
            Vector3 adjustedMovement = transform.position + movement * Time.deltaTime;
            adjustedMovement.z = adjustedMovement.y;
            transform.position = adjustedMovement;
        }
        #endregion
        #endregion
    }
}