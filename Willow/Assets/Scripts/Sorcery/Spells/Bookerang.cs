﻿using nl.SWEG.Willow.Entities.Stats;
using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.UI;
using nl.SWEG.Willow.Utils.Functions;
using UnityEngine;

namespace nl.SWEG.Willow.Sorcery.Spells
{
    /// <summary>
    /// Bookerang is the most basic attack. The player throws Greg straight ahead
    /// </summary>
    public class Bookerang : Projectile
    {
        #region Variables
        #region Editor
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Speed at which the book rotates in flight
        /// </summary>
        [SerializeField]
        [Tooltip("Speed at which the book rotates in flight")]
        private float spinSpeed;
        /// <summary>
        /// Duration for which the book pauses before returning
        /// </summary>
        [SerializeField]
        [Tooltip("Duration for which the book pauses before returning")]
        private float hangTime = 0.3f;
        /// <summary>
        /// Transform of the object that contains the sprite
        /// (in a separate object so we can spin it independently)
        /// </summary>
        [SerializeField]
        [Tooltip("Transform of the object that contains the sprite")]
        private Transform spriteTransform;
        /// <summary>
        /// Sound played when the book returns without hitting anything
        /// </summary>
        [SerializeField]
        [Tooltip("Sound played when the book returns without hitting anything")]
        private AudioClip returnSound;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Private
        /// <summary>
        /// Bool for whether the book is moving forwards (false) or returning (true)
        /// </summary>
        private bool back;
        /// <summary>
        /// Bool to make the book pause in mid-air
        /// </summary>
        private bool pause;
        /// <summary>
        /// Float to keep track of how far we've moved
        /// </summary>
        private float movedSpace;
        /// <summary>
        /// Sprite of the "crosshair" book
        /// </summary>
        private SpriteRenderer bookRenderer;        
        /// <summary>
        /// Current location of the player, so we can return to it
        /// </summary>
        private Transform playerLocation;
        /// <summary>
        /// Position where we ended the forward movement
        /// </summary>
        private Vector3 savedPosition;
        #endregion
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Get references for the player position and renderer of the "crosshair" book
        /// </summary>
        protected override void Start()
        {
            base.Start();
            if (PlayerManager.Exists)
            {
                PlayerManager player = PlayerManager.Instance;
                // Get the location of the player (to return to later)
                playerLocation = player.transform;
                // Turn the "crosshair" book invisible so it looks like this projectile IS the book
                bookRenderer = player.BookRenderer;
                bookRenderer.enabled = false;
            }
        }
        #endregion

        #region Protected
        /// <summary>
        /// Move in a straight line until we hit something or max out our range
        /// Wait for a short time, then return to the player
        /// </summary>
        protected override void Move()
        {
            // Spin Book
            spriteTransform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime,Space.World);
            // If not paused in mid-air
            if (!pause)
            {
                // If not returning to player
                if (!back)
                    FlyForward();
                // If returning to the player
                else
                    ReturnToPlayer();
            }
        }

        /// <summary>
        /// Instead of deleting itself, return the projectile to the player
        /// </summary>
        /// <param name="collision">Object the projectile collided with</param>
        protected override void Effect(Collider2D collision)
        {
            //Bool check so it doesn't get stuck on anything on the way back
            if (!back)
            {
                //play impact sound
                if (data.ImpactClip != null)
                {
                    AudioManager.Instance.PlaySound(data.ImpactClip);
                }

                // Apply knockback
                Rigidbody2D body = collision.gameObject.GetComponent<Rigidbody2D>();
                body.AddForce(transform.up * data.Knockback);
                //apply damage
                collision.gameObject.GetComponent<IHealth>()?.Damage(data.Damage);
                Return();
            }
        }

        /// <summary>
        /// Called when Bookerang hits a Wall. Returns to Player
        /// </summary>
        /// <param name="collision">Wall that was hit</param>
        protected override void HitWall(Collider2D collision)
        {
            Return();
        }
        #endregion

        #region Private
        /// <summary>
        /// Returns the book to the player, after hanging in the air for a short time
        /// </summary>
        private void Return()
        {
            back = true;
            savedPosition = transform.position;
            movedSpace = 0;
            pause = true;
            StartCoroutine(CoroutineMethods.RunDelayed(() => { pause = false; }, hangTime));
        }

        /// <summary>
        /// Fly Forward in a straight line until range is met
        /// </summary>
        private void FlyForward()
        {
            //Fly in a straight line
            base.Move();
            movedSpace += Time.deltaTime * data.ProjectileSpeed;
            if (movedSpace >= data.LifeTime) // Range has been met
            {
                // Play return sound
                AudioManager.Instance.PlaySound(returnSound);
                // Start returning to the player
                Return();
            }
        }

        /// <summary>
        /// Fly back towards the Player
        /// </summary>
        private void ReturnToPlayer()
        {
            // Use a lerp so we always target the player's current position
            transform.position = Vector3.Lerp(savedPosition, playerLocation.position, movedSpace);
            // Set speed so we always make it in time
            movedSpace += Time.deltaTime * 5;
            // If we've made it
            if (movedSpace >= 1)
            {
                // Make the "crosshair" book reappear and delete this projectile
                bookRenderer.enabled = true;
                Destroy(gameObject);
            }
        }
        #endregion
        #endregion
    }
}