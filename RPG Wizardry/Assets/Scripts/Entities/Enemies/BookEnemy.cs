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

        private float attackTimer = 0;
        #endregion

        protected override void UpdateEnemy(AvatarManager player)
        {
            // Look At Player
            Vector2 toPlayer = (Vector2)player.transform.position - (Vector2)transform.position;
            transform.right = toPlayer;

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

        }
    }
}