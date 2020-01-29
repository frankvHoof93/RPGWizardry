﻿using System.Collections;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Player.Movement
{
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
        /// Sets Movement to 0
        /// </summary>
        public void FreezeMovement()
        {
            Movement(Vector3.zero);
        }

        /// <summary>
        /// Disables the players ability to walk.
        /// </summary>
        /// <param name="duration">The stun duration</param>
        public void Stun(float duration)
        {
            StartCoroutine(StunLoop(duration));
        }

        public void ToggleMovement(bool value)
        {
            stunned = value;
            
            if(value)
            {
                animator.SetFloat("Speed", 0);
            }
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
            {
                Movement(player.InputManager.State.MovementDirection);
            }
        }
        #endregion

        #region Private
        /// <summary>
        /// Moves Avatar based on input
        /// </summary>
        /// <param name="movement">Input data in vector3 format</param>
        private void Movement(Vector3 movement)
        {
            //If the player is allowed to move
            if (!GameManager.Instance.Paused)
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
                Vector3 adjustedMovement = transform.position + movement * speed * Time.deltaTime;
                adjustedMovement.z = adjustedMovement.y;
                transform.position = adjustedMovement;
            }
            else
            {
                //Set the speed to 0 so the character stops walking.
                animator.SetFloat("Speed", 0);
            }
        }

        /// <summary>
        /// Stuns the player.
        /// </summary>
        /// <param name="duration">The stun duration</param>
        /// <returns></returns>
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