using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.Sorcery.Spells;
using nl.SWEG.Willow.Utils.Functions;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.Willow.Entities.Enemies
{
    /// <summary>
    /// Book Enemy
    /// <para>
    /// This is an immobile enemy that fires spells at the player. The spell is dropped when the enemy dies
    /// </para>
    /// </summary>
    public class BookEnemy : AEnemy
    {
        #region Variables
        #region Constants
        /// <summary>
        /// Relative modifier for spell-cooldown
        /// <para>
        /// Determines how much faster a book can use a spell, relative to a player
        /// </para>
        /// </summary>
        private const float CoolDownModifier = .5f;
        #endregion

        #region Editor
        /// <summary>
        /// Spell to use when Attacking
        /// <para>
        /// This Spell is dropped when the BookEnemy dies
        /// </para>
        /// </summary>
        [SerializeField]
        [Tooltip("Spell to use when Attacking")]
        private SpellData spell;
        /// <summary>
        /// Margin of error for Attacking (how much the angle between fwd and player-lookat can differ when attacking)
        /// </summary>
        [SerializeField]
        [Tooltip("Margin of error for Attacking (how much the angle between fwd and player-lookat can differ when attacking)")]
        private float attackAngleMargin = 5f;
        /// <summary>
        /// Modifier for turning-speed of Book
        /// <para>
        /// Angle (in degrees per second) added to base speed
        /// </para>
        /// </summary>
        [SerializeField]
        [Tooltip("Modifies the turning speed of the book (Angle (in degrees per second) added to base speed)")]
        private float turnSpeedModifier = 0;
        #endregion

        #region Private
        /// <summary>
        /// Cooldown-Timer for attacking (Time between attacks)
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

            // Vector to Player
            Vector2 toPlayer = (Vector2)player.transform.position - (Vector2)transform.position;
            toPlayer.Normalize();
            // Calculate angle between forward & vector
            float rotationAngle = Vector2.SignedAngle(transform.right, toPlayer);
            float maxAngle = (data.Speed + turnSpeedModifier) * Time.deltaTime;
            // Rotate Transform
            if (Mathf.Abs(rotationAngle) > maxAngle) // Rotation too large for one frame
                rotationAngle = (rotationAngle >= 0 ? 1 : -1) * maxAngle;
            transform.Rotate(Vector3.forward, rotationAngle);

            // Check if Book can Attack
            if (Health > 0 && attackTimer <= 0 && Vector2.Angle(transform.right, toPlayer) < attackAngleMargin)
            {
                float totalCooldown = spell.Cooldown * CoolDownModifier;
                attackTimer = totalCooldown; // Reset Cooldown
                // Attack Player
                Attack();
            }
        }

        /// <summary>
        /// Sets Rotation to Animator
        /// </summary>
        protected override void AnimateEnemy()
        {
            // Rotation from 0-3 where 0 => right & 1 => up
            int rotation = Mathf.RoundToInt(transform.rotation.eulerAngles.z / 90f) % 4;
            enemyAnimator.SetInteger("Rotation", rotation);
        }

        /// <summary>
        /// Spawns Spell held by Book (if Player does not own it yet)
        /// </summary>
        protected override void OnDeath()
        {
            if (PlayerManager.Exists)
            {
                if (!PlayerManager.Instance.Inventory.HasSpell(spell)) // Only Spawn Spell if player does not have it yet
                    LootSpawner.Instance.SpawnPage(transform.position, spell);
                else
                    LootSpawner.Instance.SpawnLoot(Collectables.Collectables.Dust, transform.position, 50); // Spawn 50 dust instead of a page
            }
            transform.parent = null;
            enemyAnimator.SetBool("Death", true);
        }
        #endregion

        #region Unity
        /// <summary>
        /// Sets initial values to Animator
        /// </summary>
        protected override void Start()
        {
            base.Start();
            enemyAnimator.SetInteger("ElementType", (int)spell.Element);
            enemyAnimator.SetBool("Attacking", false);
            AnimateEnemy();
            // Fix first frame sprite (Unity-Bug)
            string stateName = "BookFaceRight";
            switch (spell.Element)
            {
                case Element.Fire:
                    stateName = "Fire" + stateName;
                    break;
                case Element.Water:
                    stateName = "Water" + stateName;
                    break;
                case Element.Electricity:
                    stateName = "Electric" + stateName;
                    break;
                case Element.None:
                case Element.Earth:
                    stateName = "Earth" + stateName;
                    break;
            }
            enemyAnimator.Play(stateName); // Set state to animator
            enemyAnimator.Update(0);
        }
        #endregion

        #region Private
        /// <summary>
        /// Destroys book after animation has been preformed
        /// <para>
        /// Called from Unity Animation
        /// </para>
        /// </summary>
        private void DeathAnimationEnd()
        {
            Destroy(gameObject);
        }
        /// <summary>
        /// Performs attack on Player
        /// </summary>
        private void Attack()
        {
            enemyAnimator.SetBool("Attacking", true);
            List<Projectile> projectiles = spell.SpawnSpell(transform.position + transform.right * 0.2f, transform.right, attackCollisionMask);
            for (int i = 0; i < projectiles.Count; i++)
                projectiles[i].transform.localScale *= .75f; // Fire Small-Scale objects (compared to what the Player fires)
            StartCoroutine(CoroutineMethods.RunDelayed(() => {enemyAnimator.SetBool("Attacking", false);}, .75f));
        }
        #endregion
        #endregion
    }
}