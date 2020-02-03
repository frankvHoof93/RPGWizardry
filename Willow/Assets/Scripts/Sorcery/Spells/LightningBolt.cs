using nl.SWEG.Willow.Entities.Stats;
using nl.SWEG.Willow.UI;
using UnityEngine;

namespace nl.SWEG.Willow.Sorcery.Spells
{
    /// <summary>
    /// A LightningBolt shoots out a line from the Player, hitting enemies in its path
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class LightningBolt : Projectile
    {
        /// <summary>
        /// Visual representation of the lightning bolt
        /// Could be replaced by art at a later time
        /// </summary>
        private LineRenderer lineRenderer;

        /// <summary>
        /// Inherits collision layer from base class
        /// Draws a raycast, then draws a yellow line to the thing it hits, or the max distance
        /// Damages any object with health that the raycast hits
        /// </summary>
        protected override void Start()
        {
            // Call base start so we have the collision layer
            base.Start();
            // Play Impact Sound effect
            AudioManager.Instance.PlaySound(data.ImpactClip);
            // Get the line renderer
            lineRenderer = GetComponent<LineRenderer>();
            // Set Starting-Point for Line
            lineRenderer.SetPosition(0, transform.localPosition);
            // Build a raycast
            // CircleCast to make the hit more generous
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.6f, transform.up, data.LifeTime, collisionLayer);
            // Does the ray intersect any objects in the collision layer
            // Yes, it hit something
            if (hit)
            {
                //if it's an object with a rigidbody, apply knockback
                Rigidbody2D body = hit.transform.GetComponent<Rigidbody2D>();
                body?.AddForce(transform.up * data.Knockback);
                //If it's an object with health, damage it
                hit.transform.GetComponent<IHealth>()?.Damage(data.Damage);
                //Set the line to end at the object
                lineRenderer.SetPosition(1, hit.point);
            }
            //No, it didn't hit anything
            else
            {
                //Set the line to end at the max distance
                lineRenderer.SetPosition(1, transform.position + transform.up * data.LifeTime);
            }
            lineRenderer.enabled = true;
            // Attack complete; destroy self after displaying for a short time
            Destroy(gameObject,0.1f);
        }
    }
}