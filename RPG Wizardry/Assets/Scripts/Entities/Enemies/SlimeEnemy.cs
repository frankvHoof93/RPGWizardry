using System.Collections;
using System.Collections.Generic;
using nl.SWEG.RPGWizardry.Entities.Stats;
using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Utils.Functions;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Entities.Enemies
{
    public class SlimeEnemy : AEnemy
    {
        #region Variables
        /// <summary>
        /// Is this a momma or a babbu slime?
        /// </summary>
        [SerializeField]
        [Tooltip("Should this slime spawn babies?")]
        private bool big;
        /// <summary>
        /// Baby slime prefab for spawning purposes
        /// </summary>
        [SerializeField]
        [Tooltip("Baby slime prefab to spawn on death")]
        private GameObject babySlime;
        /// <summary>
        /// Current movement, to send to animator
        /// </summary>
        private Vector2 movement;
        /// <summary>
        /// Has this enemy died?
        /// </summary>
        private bool dead;
        #endregion

        #region Methods
        #region Protected
        /// <summary>
        /// Sends current movement to animator, to change animations
        /// </summary>
        /// <param name="player">Reference to Player</param>
        protected override void AnimateEnemy()
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        /// <summary>
        /// Runs AI for BookEnemy; always approach the player
        /// </summary>
        /// <param name="player">Reference to Player</param>
        protected override void UpdateEnemy(PlayerManager player)
        {
            //cant move if you're dead
            if(!dead)
            {
                //Move to Player
                movement = (Vector2)player.transform.position - (Vector2)transform.position;
                movement.Normalize();
                Vector3 adjustedMovement = transform.position + (Vector3)movement * data.Speed * Time.deltaTime;
                adjustedMovement.z = adjustedMovement.y;
                transform.position = adjustedMovement;
            }
        }

        /// <summary>
        /// Triggered when slime dies, updates animator and spawns babies if big
        /// </summary>
        /// <param name="player">Reference to Player</param>
        protected override void OnDeath()
        {
            GetComponent<Collider2D>().enabled = false;
            dead = true;
            animator.SetBool("Dead", true);
            if (big)
            {
                SpawnBabies();
            }
        }
        #endregion
        #region Private
        /// <summary>
        /// Destroys self at the end of the death animation
        /// </summary>
        private void DestroySelf()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Spawns two baby slimes at current position
        /// </summary>
        private void SpawnBabies()
        {
            Instantiate(babySlime, transform.position + new Vector3(0.2f, 0, 0), transform.rotation);
            Instantiate(babySlime, transform.position + new Vector3(-0.2f, 0, 0), transform.rotation);
        }
        
        /// <summary>
        /// If it touches an object in the target layer, it damages it
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (attackCollisionMask.HasLayer(collision.gameObject.layer))
            {
                collision.gameObject.GetComponent<IHealth>()?.Damage(data.Attack);
            }
        }
        #endregion
        #endregion
    }
}