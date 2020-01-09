using nl.SWEG.RPGWizardry.Entities.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Sorcery.Spells
{
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
            //Call base start so we have the collision layer
            base.Start();

            //Get the line renderer
            lineRenderer = GetComponent<LineRenderer>();

            //Build a raycast
            RaycastHit2D hit;
            //Does the ray intersect any objects in the collision layer
            //Yes, it hit something
            hit = Physics2D.Raycast(transform.position, transform.up, data.LifeTime, collisionLayer);
            if (hit)
            {
                //If it's an object with health, damage it
                hit.transform.GetComponent<IHealth>()?.Damage(data.Damage);
                //Set the line to end at the object
                lineRenderer.SetPosition(0, transform.localPosition);
                lineRenderer.SetPosition(1, hit.point);
            }
            //No, it didn't hit anything
            else
            {
                //Set the line to end at the max distance
                lineRenderer.SetPosition(0, transform.localPosition);
                lineRenderer.SetPosition(1, transform.position + transform.up * data.LifeTime);
            }

            //enable renderer so line appears
            lineRenderer.enabled = true;
            //attack complete; destroy self
            Destroy(gameObject,0.1f);
        }
    }
}


