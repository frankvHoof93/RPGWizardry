using nl.SWEG.RPGWizardry.Entities.Stats;
using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Utils.Functions;
using System.Collections;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Entities.Enemies
{
    public class SlimeEnemy : AEnemy
    {
        #region Variables
        #region Public
        /// <summary>
        /// Opacity-Radius in Pixels (for 720p)
        /// </summary>
        public override float OpacityRadius
        {
            get
            {
                float defaultRadius = base.OpacityRadius;
                if (!big)
                    defaultRadius *= 0.5f;
                return defaultRadius;
            }
        }
        #endregion

        #region Editor
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
        #endregion

        #region Private
        /// <summary>
        /// Current movement, to send to animator
        /// </summary>
        private Vector2 movement;
        /// <summary>
        /// Has this enemy died?
        /// </summary>
        private bool dead;
        /// <summary>
        /// Amount opponent flies back on hit
        /// </summary>
        [SerializeField]
        private int knockback;
        #endregion
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
        /// Triggered when slime dies, plays death animation
        /// </summary>
        /// <param name="player">Reference to Player</param>
        protected override void OnDeath()
        {
            dead = true;
            StartCoroutine(DieAnimation());
        }
        #endregion

        #region Private

        /// <summary>
        /// Little bit of delay (for knockback)
        /// Then disables collider, plays death animation, and spawns babies if big
        /// </summary>
        private IEnumerator DieAnimation()
        {
            yield return new WaitForSeconds(0.3f);
            GetComponent<Collider2D>().enabled = false;
            animator.SetBool("Dead", true);
            if (big)
            {
                SpawnBabies();
            }
        }

        /// <summary>
        /// Spawns two baby slimes at current position
        /// </summary>
        private void SpawnBabies()
        {
            AEnemy[] enemies = transform.GetComponentsInChildren<AEnemy>(true);
            Transform enemyParent = transform.parent;
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != this)
                {
                    enemies[i].transform.SetParent(enemyParent, true);
                    enemies[i].gameObject.SetActive(true);
                }
            }
            transform.SetParent(null);
        }

        /// <summary>
        /// Destroys self at the end of the death animation
        /// </summary>
        private void DestroySelf()
        {
            transform.parent = null;
            Destroy(gameObject);
        }
        
        /// <summary>
        /// If it touches an object in the target layer, it damages it
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (attackCollisionMask.HasLayer(collision.gameObject.layer))
            {
                collision.gameObject.GetComponent<IHealth>()?.Damage(big ? data.Attack : (ushort)(data.Attack * 0.5f));
                Rigidbody2D body = collision.gameObject.GetComponent<Rigidbody2D>();
                body.AddForce(movement * knockback);
            }
        }
        #endregion
        #endregion
    }
}