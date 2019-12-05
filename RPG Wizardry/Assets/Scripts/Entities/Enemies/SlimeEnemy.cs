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
        [SerializeField]
        private bool big;
        [SerializeField]
        private GameObject babySlime;
        private Vector2 movement;

        protected override void AnimateEnemy()
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        protected override void UpdateEnemy(PlayerManager player)
        {
            // Move to Player
            movement = (Vector2)player.transform.position - (Vector2)transform.position;
            movement.Normalize();
            Vector3 adjustedMovement = transform.position + (Vector3)movement * data.Speed * Time.deltaTime;
            adjustedMovement.z = adjustedMovement.y;
            transform.position = adjustedMovement;
        }

        protected override void OnDeath()
        {
            StartCoroutine(DeathAnimation());
        }

        private IEnumerator DeathAnimation()
        {
            //play death animation here
            yield return new WaitForSeconds(0);
            if (big)
            {
                Instantiate(babySlime, transform.position + new Vector3(0.2f, 0, 0), transform.rotation);
                Instantiate(babySlime, transform.position + new Vector3(-0.2f, 0, 0), transform.rotation);
            }
            Debug.Log(Health);
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (attackCollisionMask.HasLayer(collision.gameObject.layer))
            {
                collision.gameObject.GetComponent<IHealth>()?.Damage(data.Attack);
            }
        }
    }
}