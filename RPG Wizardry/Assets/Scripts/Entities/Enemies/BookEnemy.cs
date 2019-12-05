using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Sorcery.Spells;
using nl.SWEG.RPGWizardry.Utils.Functions;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Entities.Enemies
{
    public class BookEnemy : AEnemy
    {
        #region Variables
        #region Constants
        /// <summary>
        /// Relative modifier for spell-cooldown. Determines how much faster a book can use a spell, relative to a player
        /// </summary>
        private const float cooldownModifier = .5f;
        #endregion

        #region Editor
        /// <summary>
        /// Spell to use when Attacking
        /// </summary>
        [SerializeField]
        [Tooltip("Spell to use when Attacking")]
        private SpellData spell;
        /// <summary>
        /// LayerMask for Attacks
        /// </summary>
        [SerializeField]
        [Tooltip("LayerMask for Attacks")]
        private LayerMask spellCollisionMask;
        /// <summary>
        /// Margin of error for Attacking (how much the angle between fwd and player-lookat can differ when attacking)
        /// </summary>
        [SerializeField]
        [Tooltip("Margin of error for Attacking (how much the angle between fwd and player-lookat can differ when attacking)")]
        private float attackAngleMargin = 5f;
        #endregion

        #region Private
        /// <summary>
        /// Cooldown-Timer for Attacking
        /// </summary>
        private float attackTimer;
        #endregion
        #endregion

        #region Methods
        #region Protected
        /// <summary>
        /// Runs AI for BookEnemy
        /// </summary>
        /// <param name="player">Reference to Player</param>
        protected override void UpdateEnemy(PlayerManager player)
        {
            // Run Cooldown-Timer
            if (attackTimer > 0)
                attackTimer -= Time.deltaTime;
            // Look At Player
            Vector2 toPlayer = (Vector2)player.transform.position - (Vector2)transform.position;
            toPlayer.Normalize();
            float rotationAngle = Vector2.SignedAngle(transform.right, toPlayer);
            float maxAngle = data.Speed * Time.deltaTime;

            if (Mathf.Abs(rotationAngle) <= maxAngle) // Full rotation
            {
                // angle for Fwd
                float absAngle = Vector2.SignedAngle(Vector2.up, toPlayer);
                // Get up from fwd
                absAngle += 90f;
                // Set up to transform
                transform.rotation = Quaternion.Euler(0, 0, absAngle);
            }                
            else // Partial rotation
            {
                if (rotationAngle < 0)
                    maxAngle *= -1f;
                transform.Rotate(Vector3.forward, maxAngle);
            }
            // Check if Book is looking at Player
            rotationAngle = Vector2.Angle(transform.right, toPlayer);
            if (rotationAngle >= attackAngleMargin) // Not looking at player
                return;
            // Check if Book can Attack
            if (attackTimer <= 0)
            {
                float totalCooldown = spell.Cooldown * cooldownModifier;
                attackTimer = totalCooldown;
                // Attack Player
                Attack();
            }
        }
        #endregion

        #region Unity
        /// <summary>
        /// Sets ElementType to Animator
        /// </summary>
        protected override void Start()
        {
            base.Start();
            animator.SetInteger("ElementType", (int)spell.Element);
            animator.SetBool("Attacking", false);
            AnimateEnemy();
        }

        /// <summary>
        /// Sets Rotation to Animator
        /// </summary>
        protected override void AnimateEnemy()
        {
            // Rotation from 0-3 where 0 => right & 1 => up
            int rotation = Mathf.RoundToInt(transform.rotation.eulerAngles.z / 90f) % 4;
            animator.SetInteger("Rotation", rotation);
        }
        #endregion

        #region Private
        /// <summary>
        /// Performs attack
        /// </summary>
        private void Attack()
        {
            animator.SetBool("Attacking", true);
            List<Projectile> projectiles = spell.SpawnSpell(transform.position + transform.right * 0.2f, transform.right, spellCollisionMask);
            for (int i = 0; i < projectiles.Count; i++)
                projectiles[i].transform.localScale *= .75f; // Fire Small-Scale objects (compared to what the Player fires)
            StartCoroutine(CoroutineMethods.RunDelayed(() => {animator.SetBool("Attacking", false);}, .75f));
        }
        #endregion
        #endregion
    }
}