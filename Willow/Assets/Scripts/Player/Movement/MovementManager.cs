using nl.SWEG.Willow.GameWorld;
using System.Collections;
using UnityEngine;

namespace nl.SWEG.Willow.Player.Movement
{
    /// <summary>
    /// Handles Movement for Player
    /// </summary>
    [RequireComponent(typeof(Animator), typeof(PlayerManager))]
    public class MovementManager : MonoBehaviour
    {
        #region Variables
        #region Editor
        /// <summary>
        /// MovementSpeed for Player
        /// </summary>
        [SerializeField]
        [Tooltip("MovementSpeed for Player")]
        private float speed = 1f;
        #endregion

        #region Private
        /// <summary>
        /// Animator for Avatar
        /// </summary>
        private Animator animator;
        /// <summary>
        /// Manager for Player
        /// </summary>
        private PlayerManager player;
        /// <summary>
        /// Whether the Player is allowed to move
        /// </summary>
        private bool stunned;
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Disables the players ability to walk.
        /// </summary>
        /// <param name="duration">The stun duration</param>
        public void Stun(float duration)
        {
            StartCoroutine(StunLoop(duration));
        }

        /// <summary>
        /// Sets Stunned-Value for Player
        /// </summary>
        /// <param name="stunned">True to stun Player</param>
        public void SetStunned(bool stunned)
        {
            this.stunned = stunned;            
            if(stunned)
                animator.SetFloat("Speed", 0);
        }
        #endregion

        #region Unity
        /// <summary>
        /// Grabs Reference to Animator and InputState
        /// </summary>
        private void Start()
        {
            animator = GetComponent<Animator>();
            player = GetComponent<PlayerManager>();
        }

        /// <summary>
        /// Handles Movement based on Input
        /// </summary>
        private void FixedUpdate()
        {
            if (GameManager.Exists && !GameManager.Instance.Paused && !stunned)
                Movement(player.InputManager.State.MovementDirection);
            else
                animator.SetFloat("Speed", 0); // Set the speed to 0 so the character stops walking
        }
        #endregion

        #region Private
        /// <summary>
        /// Moves Avatar based on Input
        /// </summary>
        /// <param name="movement">Input data in vector3 format</param>
        private void Movement(Vector3 movement)
        {
            // Send values to the animator so it can decide what animation to show
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.magnitude);
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
            // Move the Avatar
            Vector3 adjustedMovement = transform.position + movement * speed * Time.deltaTime;
            transform.position = adjustedMovement;
        }

        /// <summary>
        /// Stuns the player
        /// </summary>
        /// <param name="duration">Duration Player is stunned for</param>
        private IEnumerator StunLoop(float duration)
        {
            stunned = true;
            yield return new WaitForSeconds(duration);
            stunned = false;
        }
        #endregion
        #endregion
    }
}