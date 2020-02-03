using nl.SWEG.Willow.Entities.Stats;
using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.Utils.Functions;
using System.Collections;
using UnityEngine;

namespace nl.SWEG.Willow.Entities.Enemies
{
    /// <summary>
    /// Slime Enemy
    /// <para>
    /// This enemy tries to damage the Player by running into them
    /// </para>
    /// </summary>
    public class SlimeEnemy : AEnemy
    {
        #region Variables
        #region Public
        /// <summary>
        /// Opacity-Radius in Pixels (for 720p-Resolution)
        /// </summary>
        public override float OpacityRadius
        {
            get
            {
                float radius = base.OpacityRadius;
                if (!big)
                    radius *= 0.5f;
                return radius;
            }
        }
        #endregion

        #region Editor
        /// <summary>
        /// Whether this is a big slime (Big slimes spawn smaller slimes upon death)
        /// </summary>
        [SerializeField]
        [Tooltip("Whether this is a big slime (Big slimes spawn smaller slimes upon death)")]
        private bool big;
        #endregion

        #region Private
        /// <summary>
        /// Movement during current frame (sent to animator)
        /// </summary>
        private Vector2 movement = Vector2.zero;
        #endregion
        #endregion

        #region Methods
        #region Protected
        /// <summary>
        /// Sends current movement to animator
        /// </summary>
        protected override void AnimateEnemy()
        {
            enemyAnimator.SetFloat("Horizontal", movement.x);
            enemyAnimator.SetFloat("Vertical", movement.y);
        }

        /// <summary>
        /// Runs AI for BookEnemy. Attempts to approach the player
        /// </summary>
        /// <param name="player">Reference to Player</param>
        protected override void UpdateEnemy(PlayerManager player)
        {
            // Can't move if you're dead
            if(Health > 0)
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
        /// Triggered when slime dies, starts death animation
        /// </summary>
        protected override void OnDeath()
        {
            StartCoroutine(DieAnimation());
        }
        #endregion

        #region Unity
        /// <summary>
        /// If it touches an object in the target layer, it damages it
        /// </summary>
        /// <param name="collision">Collision that occurred</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (attackCollisionMask.HasLayer(collision.gameObject.layer))
            {
                collision.gameObject.GetComponent<IHealth>()?.Damage(big ? data.Attack : (ushort)(data.Attack * 0.5f));
                Rigidbody2D body = collision.gameObject.GetComponent<Rigidbody2D>();
                body?.AddForce(movement * (big ? data.Knockback : (data.Knockback * 0.75f)));
            }
        }
        #endregion

        #region Private
        /// <summary>
        /// Disables collider, plays death animation, and spawns babies if big
        /// <para>
        /// Small delay of .3 seconds (for knockback to run) before running
        /// </para>
        /// </summary>
        private IEnumerator DieAnimation()
        {
            yield return new WaitForSeconds(0.3f);
            GetComponent<Collider2D>().enabled = false;
            enemyAnimator.SetBool("Dead", true);
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
            Transform enemyParent = transform.parent;
            for (int i = 0; i < 2; i++)
            {
                SlimeEnemy baby = Instantiate(gameObject).GetComponent<SlimeEnemy>();
                baby.big = false;
                baby.transform.SetParent(enemyParent);
                baby.transform.position = transform.position + (Vector3)(Random.insideUnitCircle * .2f);
                baby.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                baby.GetComponent<Collider2D>().enabled = true;
            }
            transform.SetParent(null);
        }

        /// <summary>
        /// Destroys this object
        /// <para>
        /// Called by Unity Animator
        /// </para>
        /// </summary>
        private void DestroySelf()
        {
            transform.parent = null; // Set parent null before death-event is invoked
            Destroy(gameObject);
        }
        #endregion
        #endregion
    }
}