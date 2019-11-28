using nl.SWEG.RPGWizardry.Avatar;
using nl.SWEG.RPGWizardry.Sorcery.Spells;
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

        [SerializeField]
        private SpellData spell;
        [SerializeField]
        private LayerMask spellCollisionMask;

        [SerializeField]
        private float attackAngleMargin = 5f;

        private float attackTimer = 0;
        #endregion

        protected override void UpdateEnemy(AvatarManager player)
        {
            // Run Cooldown-Timer
            if (attackTimer > 0)
                attackTimer -= Time.deltaTime;
            // Look At Player
            Vector2 toPlayer = (Vector2)player.transform.position - (Vector2)transform.position;
            float rotationAngle = Vector2.SignedAngle(transform.right, toPlayer);
            float maxAngle = data.Speed * Time.deltaTime;
            if (Mathf.Abs(rotationAngle) <= maxAngle) // Full rotation
                transform.right = toPlayer;
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

        private void Attack()
        {
            Vector2 fireDir = transform.right;
            spell.SpawnSpell(transform.position + transform.right * 0.2f, transform.right, spellCollisionMask);
        }
    }
}